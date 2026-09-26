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

        protected override float FireRate => _data.FireRate;

        private LineRenderer _beamRenderer;
        private bool _isfiring;
        private Vector3 _startBeamWorldPosition;
        private Vector3 _endBeamWorldPosition;
        private float _currentLength;
        private Collider[] _hitColliders;
        private HashSet<int> _damagedTargetIds;

        protected override void OnAwake()
        {
            AttackRange = _data.MaxRange;

            TryGetComponent(out _beamRenderer);
            _beamRenderer.enabled = false;
            _hitColliders = new Collider[_data.MaxHitCount];
            _damagedTargetIds = new(_data.MaxHitCount);
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

            _beamRenderer.enabled = true;
            _beamRenderer.useWorldSpace = true;

            int hitCount = Physics.OverlapCapsuleNonAlloc(
               _startBeamWorldPosition,
               _endBeamWorldPosition,
               _beamRenderer.startWidth,
               _hitColliders
            );

            if (hitCount > 0)
            {
                for (int i = 0; i < hitCount; i++)
                {
                    if (_damagedTargetIds.Add(_hitColliders[i].GetInstanceID()))
                    {
                        _hitColliders[i].TryGetComponent(out IDamageable target);
                        target?.GetDamage(_data.Damage);
                    }
                }
            }

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
            if (_isFired) return;

            _startBeamWorldPosition = _muzzlePoint.position;
            _damagedTargetIds.Clear();
            _isfiring = true;
            _isFired = true;
        }
    }
}