using UnityEngine;

namespace RobotAction.Gameplay.Parts.Weapons.Guns
{
    public readonly struct BulletContext
    {
        public readonly LayerMask AttackTargetLayer;
        public readonly float LifeTime { get; }
        public readonly float Speed { get; }
        public readonly float BaseAttackPower { get; }
        
        public BulletContext(LayerMask targetLayer,float lifeTime,float speed, float baseAttackPower)
        {
            AttackTargetLayer = targetLayer;
            LifeTime = lifeTime;
            Speed = speed;
            BaseAttackPower = baseAttackPower;
        }
    }
}
