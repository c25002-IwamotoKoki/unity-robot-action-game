using UnityEngine;

namespace RobotAction.Gameplay.Weapons.Guns
{
    [RequireComponent(typeof(BulletPool))]
    public abstract class MagazineBase : MonoBehaviour
    {
        public BulletPool BulletPool { get; private set; }

        private void Awake()
        {
            if(TryGetComponent(out BulletPool bulletPool))
            {
                BulletPool = bulletPool;
            }
        }
    }

}
