using RobotAction.Gameplay.Combat;
using UnityEngine;

namespace RobotAction.Gameplay.Parts.Weapons.Guns
{
    [RequireComponent(typeof(LineRenderer))]
    public class EnergyGun : GunBase
    {
        [SerializeField] private Transform _muzzlePoint;
        [SerializeField] private float _fireRate;
        [SerializeField] private float _damage;
        [SerializeField] private float _maxRange;
        [SerializeField] private float _extendSpeed;
        [SerializeField] private float _attackRange;
        [SerializeField] private LayerMask _hitLayer;

        protected override float FireRate => _fireRate;

        private LineRenderer _beamRenderer;
        private bool _isfiring;
        private Vector3 _targetPosition;
        private Vector3 _startBeamWorldPosition;
        private Vector3 _endBeamWorldPosition;
        private float _currentLength;
        private IDamageable _hitTarget;

        protected override void OnAwake()
        {
            _beamRenderer.enabled = false;
            AttackRange = _maxRange;
            TryGetComponent(out _beamRenderer);
            base.OnAwake();
        }

        protected override void Update()
        {
            base.Update();

            if (!_isfiring) return;

            float toTargetDistance = Vector3.Distance(_muzzlePoint.position,_targetPosition);
            _currentLength += _extendSpeed * Time.deltaTime;

            if(_currentLength >= _maxRange)
            {
                _currentLength = 0;
                _targetPosition = Vector3.zero;
                _hitTarget?.GetDamage(_damage);     
                _isfiring = false;
                _beamRenderer.enabled = false;
            }

            _endBeamWorldPosition = _startBeamWorldPosition + transform.forward * _currentLength;

            _beamRenderer.SetPosition(0,_startBeamWorldPosition);
            _beamRenderer.SetPosition(1,_endBeamWorldPosition);
        }

        public override void SetTarget(Vector3 position)
        {
            _muzzlePoint.LookAt(position);
        }

        public override void Fire()
        {
            if (_isFired) return;

            if(Physics.Raycast(_muzzlePoint.position,
                               _muzzlePoint.forward,
                               out RaycastHit hit,
                               _maxRange,
                               _hitLayer))
            {
                hit.collider.TryGetComponent(out _hitTarget);
                _targetPosition = hit.transform.position;
            }
            else
            {
                _targetPosition.z = _maxRange;
            }

            _startBeamWorldPosition = _muzzlePoint.position;

            _beamRenderer.enabled = true;
            _beamRenderer.useWorldSpace = true;
            _isfiring = true;
            _isFired = true;
        }
    }
}
