using UnityEngine;

namespace RobotAction.Gameplay.Weapons.Guns
{
    public abstract class GunBase : MonoBehaviour
    {
        protected abstract float FireRate { get; }

        protected float _fireRateTimer;
        protected bool _canShoot;

        protected virtual void Update()
        {
            if (!_canShoot)
            {
                _fireRateTimer += Time.deltaTime;

                if (_fireRateTimer >= FireRate)
                {
                    _canShoot = true;
                    _fireRateTimer = 0;
                }
            }
        }

        protected void ResetCollDown()
        {
            _fireRateTimer = 0;
            _canShoot = true;
        }

        public abstract void Shoot();
    }
}
