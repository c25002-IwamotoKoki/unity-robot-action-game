using UnityEngine;

namespace RobotAction.Gameplay.Parts.Weapons.Guns
{
    public abstract class GunBase : WeaponPartBase
    {
        protected abstract float FireRate { get; }

        protected bool CanFire => RemainingUseCount > 0;
        protected bool _isFired;

        private float _fireRateTimer;

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

        public override void Attack() => Fire();

        public abstract void Fire();
    }
}
