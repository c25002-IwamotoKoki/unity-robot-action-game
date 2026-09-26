namespace RobotAction.Gameplay.Player
{
    public interface IPlayerMoverData
    {
        public float MoveSpeed { get; }
        public float DefaultMaxSpeed { get; }
        public float MoveEnergyCost { get; }
        public float BoostSpeed { get; }
        public float BoostEnergyCost { get; }
        public float BoostMaxSpeed { get; }
        public float MaxSpeedDeceleration { get; }
    }
}
