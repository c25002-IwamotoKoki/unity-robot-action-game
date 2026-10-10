using RobotAction.Gameplay.Combat;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RobotAction.Gameplay.Parts.Weapons.Guns
{
    [RequireComponent(typeof(LineRenderer))]
    public class EnergyGun : GunBase
    {
        [SerializeField] private Transform _muzzlePoint;
        [SerializeField] private EnergyGunData _data;

        public override int MaxUseCount => _data.MaxAmmo;

        protected override float FireRate => _data.FireRate;

        private LineRenderer _beamRenderer;
        private Collider[] _hitColliders;
        private HashSet<int> _damagedTargetIds;
        private Vector3 _startBeamWorldPosition;
        private Vector3 _endBeamWorldPosition;
        private float _currentLength;
        private bool _isfiring;

        protected override void OnAwake()
        {
            AttackRange = _data.MaxRange;

            TryGetComponent(out _beamRenderer);
            _beamRenderer.enabled = false;
            _hitColliders = new Collider[_data.MaxHitCount];
            _damagedTargetIds = new(_data.MaxHitCount);
            RemainingUseCount = _data.MaxAmmo;
            base.OnAwake();
        }

        protected override void Update()
        {
            base.Update();

            if (!_isfiring) return;

            _currentLength += _data.BeamExtendSpeed * Time.deltaTime;
            _currentLength = MathF.Min(_data.MaxRange, _currentLength);

            _endBeamWorldPosition = _startBeamWorldPosition + transform.forward * _currentLength;

            _beamRenderer.SetPosition(0, _startBeamWorldPosition);
            _beamRenderer.SetPosition(1, _endBeamWorldPosition);

            //ここでRendererをアクティブにしているのはFire()関数内で書くと
            //ビームが伸びる前の状態が見えるため視覚的に違和感を感じるため
            _beamRenderer.enabled = true;
            _beamRenderer.useWorldSpace = true;

            BeamHitProcess();

            if (_currentLength >= _data.MaxRange)
            {
                _currentLength = 0;

                _isfiring = false;
                _beamRenderer.enabled = false;
            }
        }

        public override void SetTarget(Vector3 position)
        {
            _muzzlePoint.LookAt(position);
        }

        public override void Fire()
        {
            if (_isFired || !CanFire) return;

            _startBeamWorldPosition = _muzzlePoint.position;
            _damagedTargetIds.Clear();
            RemainingUseCount--;
            InvokeOnWeaponStatusChangedEvent();
            _isfiring = true;
            _isFired = true;
        }

        private void BeamHitProcess()
        {
            int hitCount = Physics.OverlapCapsuleNonAlloc(
               _startBeamWorldPosition,
               _endBeamWorldPosition,
               _beamRenderer.startWidth,
               _hitColliders
            );

            if (hitCount <= 0) return;

            SortHitCollidersByProximity(hitCount);

            for (int i = 0; i < hitCount; i++)
            {
                //自身に当たった場合はスキップ
                if (_hitColliders[i].transform == transform.root) continue;

                //_hitCollidersは近い順に並んでいるため近い順にダメージを与えていき
                //何かのオブジェクトにぶつかった時点でビームを止める処理
                if (_hitColliders[i].TryGetComponent(out IDamageable target))
                {
                    //同じ敵に多重にヒットすることを防止する処理
                    if (_damagedTargetIds.Add(_hitColliders[i].GetInstanceID()))
                    {
                        target?.GetDamage(_data.Damage);
                    }
                }
                else
                {
                    _currentLength = 0;

                    _isfiring = false;
                    _beamRenderer.enabled = false;
                    return;
                }
            }
        }

        private void SortHitCollidersByProximity(int hitCount)
        {
            for (int i = 0; i < hitCount - 1; i++)
            {
                for (int j = 0; j < hitCount; j++)
                {
                    float distanceSqrI = (_hitColliders[i].transform.position - _startBeamWorldPosition).sqrMagnitude;
                    float distanceSqrJ = (_hitColliders[j].transform.position - _startBeamWorldPosition).sqrMagnitude;

                    if (distanceSqrI > distanceSqrJ)
                    {
                        var temp = _hitColliders[j];
                        _hitColliders[j] = _hitColliders[i];
                        _hitColliders[i] = temp;
                    }
                }
            }
        }
    }
}