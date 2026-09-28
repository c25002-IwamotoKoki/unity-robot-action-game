namespace RobotAction.Core
{
    public readonly struct PlayerEnergyInfo
    {
        public float MaxEnergy { get; }
        public float CurrentEnergy { get; }

        public PlayerEnergyInfo(float maxEnergy, float currentEnergy)
        {
            MaxEnergy = maxEnergy;
            CurrentEnergy = currentEnergy;
        }
    }
}
