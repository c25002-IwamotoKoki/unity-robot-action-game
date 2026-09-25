using RobotAction.Gameplay.Parts;
using UnityEngine;

namespace RobotAction.Gameplay.Parts.Weapons.Guns
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public abstract class GunBase : MonoBehaviour,IWeaponPart
    {
        public Transform Owner { get; private set; }

        public float AttackRange { get; protected set; }

        protected abstract float FireRate { get; }

        private float _fireRateTimer;
        protected bool _isFired;

        private Rigidbody _rigidbody;
        private Collider _collider;

        private void Awake()
        {
            TryGetComponent(out _rigidbody);
            TryGetComponent(out _collider);

            OnAwake();
        }

        protected virtual void Update()
        {
            if (_isFired)
            {
                _fireRateTimer += Time.deltaTime;

                if (_fireRateTimer >= FireRate)
                {
                    _isFired = false;
                    _fireRateTimer = 0;
                }
            }
        }

        protected virtual void OnAwake()
        {

        }

        public void Attack() => Fire();

        public virtual void Equip(Transform owner)
        {
            Owner = owner;
            _rigidbody.isKinematic = true;
            _collider.enabled = false;
            transform.SetParent(Owner);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }

        public void Unequip()
        {
            Owner = null;
            _rigidbody.isKinematic = false;
            _collider.enabled = true;
            transform.SetParent(null);
        }

        protected void ResetCollDown()
        {
            _fireRateTimer = 0;
            _isFired = true;
        }

        public abstract void Fire();

        public abstract void SetTarget(Vector3 position);
    }
}
