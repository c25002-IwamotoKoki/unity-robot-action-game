namespace RobotAction.Gameplay.Weapons.Guns
{
    public readonly struct BulletContext
    {
        public readonly float LifeTime { get; }
        public readonly float Speed { get; }
        public readonly float BaseAttackPower { get; }
        
        public BulletContext(float lifeTime,float speed, float baseAttackPower)
        {
            LifeTime = lifeTime;
            Speed = speed;
            BaseAttackPower = baseAttackPower;
        }
    }
}
