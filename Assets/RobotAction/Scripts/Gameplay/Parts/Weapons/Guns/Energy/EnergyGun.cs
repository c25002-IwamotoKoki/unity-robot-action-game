using RobotAction.Gameplay.Combat;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RobotAction.Gameplay.Parts.Weapons.Guns
{
    public class EnergyGun : GunBase
    {
        [SerializeField] private Transform _muzzlePoint;
        [SerializeField] private ParticleSystem _beamParticle;
        [SerializeField] private EnergyGunData _data;

        public override int MaxUseCount => _data.MaxAmmo;

        protected override float FireRate => _data.FireRate;

        private Collider[] _hitColliders;
        private HashSet<int> _damagedTargetIds;
        private Vector3 _startBeamWorldPosition;
        private Vector3 _endBeamWorldPosition;
        private float _currentLength;
        private bool _isfiring;

        protected override void OnAwake()
        {
            AttackRange = _data.MaxRange;

            _hitColliders = new Collider[_data.MaxHitCount];
            _damagedTargetIds = new(_data.MaxHitCount);
            RemainingUseCount = _data.MaxAmmo;

            if(_beamParticle == null)
            {
                Debug.LogWarning("BeamParticleが未設定です");
            }
            else
            {
                var mainModule = _beamParticle.main;
                mainModule.startLifetimeMultiplier = Mathf.Ceil(_data.MaxRange / _data.BeamExtendSpeed);
                mainModule.startSpeedMultiplier = _data.BeamExtendSpeed;
            }
           
            base.OnAwake();
        }

        protected override void Update()
        {
            base.Update();

            if (!_isfiring) return;

            _currentLength += _data.BeamExtendSpeed * Time.deltaTime;
            _currentLength = MathF.Min(_data.MaxRange, _currentLength);

            _endBeamWorldPosition = _startBeamWorldPosition + _muzzlePoint.forward * _currentLength;

            BeamHitProcess();

            if (_currentLength >= _data.MaxRange)
            {
                _currentLength = 0;

                _isfiring = false;

                if (_beamParticle != null)
                {
                    _beamParticle.Clear();
                    _beamParticle.Stop();
                }
            }
        }

        public override void SetTarget(Vector3 position)
        {
            _muzzlePoint.LookAt(position);
        }

        public override void ResetTarget()
        {
            _muzzlePoint.rotation = transform.rotation;
        }

        public override void Fire()
        {
            if (_isFired || !CanFire) return;

            _startBeamWorldPosition = _muzzlePoint.position;
            _damagedTargetIds.Clear();
            RemainingUseCount--;
            InvokeOnWeaponStatusChangedEvent();
            PlayShotSE(_data.ShotSEId);
            if (_beamParticle != null) _beamParticle.Play();
            _isfiring = true;
            _isFired = true;

        }

        private void BeamHitProcess()
        {
            LayerMask hitMask = AttackTargetLayer | _data.ObstacleLayer;

            int hitCount = Physics.OverlapCapsuleNonAlloc(
               _startBeamWorldPosition,
               _endBeamWorldPosition,
               _data.BeamWidth,
               _hitColliders,
               hitMask
            );

            if (hitCount <= 0) return;

            SortHitCollidersByProximity(hitCount);

            for (int i = 0; i < hitCount; i++)
            {
                //_hitCollidersは近い順に並んでいるため近い順にダメージを与えていき
                //攻撃対象(IDamageable)以外のオブジェクトにぶつかった時点でビームを止める処理
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
                    if (_beamParticle != null)
                    {
                        _beamParticle.Clear();
                        _beamParticle.Stop();
                    }
                    _isfiring = false;
                    return;
                }
            }
        }

        private void SortHitCollidersByProximity(int hitCount)
        {
            for (int i = 0; i < hitCount - 1; i++)
            {
                for (int j = i + 1; j < hitCount; j++)
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