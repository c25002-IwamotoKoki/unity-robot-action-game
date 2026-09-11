using RobotAction.Gameplay.ObjectPool;
using RobotAction.Gameplay.Scriptables;
using UnityEngine;

namespace RobotAction.Gameplay.Weapons.Guns
{
    public class KineticMagazine : MagazineBase
    {
        public void SetDefaultCapacity(KineticGunData gunData)
        {
            BulletPool.SetCalculateCapacity(gunData.BulletLifeTime,gunData.FireRate);
        }

        public void SpawnBullet(in BulletContext context, Vector3 position,Quaternion rotation)
        {
            var bullet = BulletPool.Pool.Get();

            if(bullet != null)
            {
                bullet.transform.SetPositionAndRotation(position,rotation);
                bullet.Shoot(context);
            }
        }
    }
}
