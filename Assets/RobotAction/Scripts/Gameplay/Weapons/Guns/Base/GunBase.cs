using UnityEngine;

namespace RobotAction.Gameplay.Weapons.Guns
{
    public abstract class GunBase : MonoBehaviour
    {
        protected abstract float FireRate { get; }

        protected float _fireRateTimer;
        protected bool _isShot;

        protected virtual void Update()
        {
            if (!_isShot)
            {
                _fireRateTimer += Time.deltaTime;

                if (_fireRateTimer >= FireRate)
                {
                    _isShot = true;
                    _fireRateTimer = 0;
                }
            }
        }

        protected void ResetCollDown()
        {
            _fireRateTimer = 0;
            _isShot = true;
        }

        public abstract void Shoot();

        public abstract void SetShootTarget(Vector3 position);
    }
}
