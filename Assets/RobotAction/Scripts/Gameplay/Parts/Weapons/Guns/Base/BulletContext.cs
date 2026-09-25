using UnityEngine;

namespace RobotAction.Gameplay.Parts.Weapons.Guns
{
    public readonly struct BulletContext
    {
        public readonly Transform Owner;
        public readonly float LifeTime { get; }
        public readonly float Speed { get; }
        public readonly float BaseAttackPower { get; }
        
        public BulletContext(Transform owner,float lifeTime,float speed, float baseAttackPower)
        {
            Owner = owner;
            LifeTime = lifeTime;
            Speed = speed;
            BaseAttackPower = baseAttackPower;
        }
    }
}
