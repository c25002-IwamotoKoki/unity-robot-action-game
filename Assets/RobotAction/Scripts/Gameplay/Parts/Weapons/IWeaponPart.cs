namespace RobotAction.Gameplay.Parts
{
    public interface IWeaponPart : IPart
    {
        public float AttackRange { get; }
        public void Attack();
    }
}
