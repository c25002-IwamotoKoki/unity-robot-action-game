using RobotAction.Gameplay.ObjectPool;
using RobotAction.Gameplay.Scriptables;
using UnityEngine;

namespace RobotAction.Gameplay.Weapons.Guns
{
    public class BulletPool : ComponentPoolBase<BulletBase>
    {
        public void SetCalculateCapacity(float bulletLifeTime, float fireRate)
        {
            _defaultCapacity = Mathf.FloorToInt( bulletLifeTime / fireRate);
            //1つ多めに確保しているのは_defautlCapacityより大きいサイズ確保しておかないと
            //GetCountAllストッパーが正しいく機能しないからです。
            _maxCapacity = _defaultCapacity + 1;
        }

        public void PrewarmPool()
        {
            PrewarmPoolFlow();
        }
    }
}
