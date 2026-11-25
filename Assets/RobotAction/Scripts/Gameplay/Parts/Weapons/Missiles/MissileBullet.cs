using Cysharp.Threading.Tasks;
using RobotAction.Gameplay.Combat;
using RobotAction.Gameplay.ObjectPool;
using UnityEngine;

namespace RobotAction.Gameplay.Parts.Weapons.Missiles
{
    public class MissileBullet : MonoBehaviour
    {
        [SerializeField] private TrailRenderer _smokeTrailRenderer;

        private StackPool<MissileBullet> _ownerPool;
        private MissileLauncherData _launcherData;
        private MeshRenderer _meshRenderer;
        private Transform _currentTarget;
        private Collider[] _hitColliders;
        private LayerMask _attackTargetLayer;
        private int _fireGeneration;
        private float _lifeTimer;
        private bool _isFlighting;
        private bool _isReleaced;        

        private void Update()
        {
            if (!_isFlighting) return;

            _lifeTimer += Time.deltaTime;
            if(_lifeTimer >= _launcherData.BulletLifeTime)
            {
                ReturnToPool();
            }
        }

        public void Initialize(StackPool<MissileBullet> ownerPool,MissileLauncherData launcherData)
        {
            _ownerPool = ownerPool;
            _launcherData = launcherData;

            _hitColliders = new Collider[_launcherData.MaxBlastHitCount];

             _meshRenderer = GetComponentInChildren<MeshRenderer>();

            if(_meshRenderer == null)
            {
                throw new MissingReferenceException("MissileBulletPrefabにMeshRendererがついていません");
            }

            _meshRenderer.enabled = false;
        }

        public void SetAttackTargetLayer(LayerMask layer)
        {
            _attackTargetLayer = layer;
        }

        public void Fire(Transform target)
        {
            if (_isFlighting) return;

            _isFlighting = true;
            _isReleaced = false;
            _currentTarget = target;
            _meshRenderer.enabled = true;
            _smokeTrailRenderer.Clear();

            FlightProcessAsync(_fireGeneration).Forget();
        }

        private async UniTaskVoid FlightProcessAsync(int generation)
        {
            //CancellationTokenSource生成によるGCを避けるため
            //Delay機能を使わず、Timer式で実質的なDelay+中断処理を行っています。
            float delayTimer = 0;
            float lifeTimer = 0;

            while (delayTimer < _launcherData.BulletToHomingDelay)
            {
                transform.position += Time.deltaTime * _launcherData.BulletFlightSpeed * transform.forward;

                if (TryHitProcess())
                {
                    ReturnToPool();
                    return;
                }

                await UniTask.Yield(PlayerLoopTiming.Update,destroyCancellationToken);
                if (generation != _fireGeneration || !isActiveAndEnabled) return;

                delayTimer += Time.deltaTime;
                lifeTimer += Time.deltaTime;
            }

            //寿命まで飛び続けるフェーズ
            while(_isFlighting && _lifeTimer < _launcherData.BulletLifeTime)
            {
                if (TryHitProcess())
                {
                    ReturnToPool();
                    return;
                }

                if (_currentTarget != null)
                {
                    Vector3 direction = _currentTarget.position - transform.position;
                    if(direction.sqrMagnitude >= 0.00001f)
                    {
                        Quaternion lookRotation = Quaternion.LookRotation(direction);
                        float diffRotation = (transform.eulerAngles - lookRotation.eulerAngles).magnitude;
                        transform.rotation = Quaternion.RotateTowards(
                            transform.rotation,
                            lookRotation,
                            diffRotation * _launcherData.BulletHomingRate * Time.deltaTime
                        );
                    }               
                }

                transform.position += Time.deltaTime * _launcherData.BulletOnHomingSpeed * transform.forward;

                await UniTask.Yield(PlayerLoopTiming.Update,destroyCancellationToken);
                if (generation != _fireGeneration || !isActiveAndEnabled) return;
            }

            //終了時に自己を返却
            ReturnToPool();
        }

        /// <summary>
        /// 命中した際内部で爆発ダメージ判定処理も行っています。
        /// </summary>
        /// <returns></returns>
        private bool TryHitProcess()
        {
            //弾の大きさ平均からヒット判定を割り出す。
            //爆発の大きさはまた別で判定するためこれはあくまでもBullet本体が命中しているかどうかの判定
            float hitRadius = (transform.localScale.x + transform.localScale.y + transform.localScale.z) / 3;

            //障害物に当たっても爆発するようにするためLayerを合成
            LayerMask hitLayer = _attackTargetLayer | _launcherData.ObstacleLayer;
            int hitCount = Physics.OverlapSphereNonAlloc(
                transform.position,
                hitRadius,
                _hitColliders,
                hitLayer
            );

            if (hitCount <= 0) return false;

            //爆発時の処理
            hitCount = Physics.OverlapSphereNonAlloc(
                transform.position,
                _launcherData.BlastRadius,
                _hitColliders,
                _attackTargetLayer
            );

            for(int i = 0; i < hitCount; i++)
            {
                if (_hitColliders[i].TryGetComponent(out IDamageable damageable))
                {
                    damageable.GetDamage(_launcherData.AttackPower);
                }
            }
            
            //ここまで到達しているなら少なくとも何かには当たっているためtrueを返す
            return true;
        }

        private void ReturnToPool()
        {
            if (_isReleaced) return;

            _isReleaced = true;
            _isFlighting = false;
            _lifeTimer = 0f;
            //番号を進めて実行中のループを無効化
            _fireGeneration++;
            _currentTarget = null;
            _smokeTrailRenderer.Clear();
            _meshRenderer.enabled = false;

            _ownerPool.Release(this);
        }
    }
}
