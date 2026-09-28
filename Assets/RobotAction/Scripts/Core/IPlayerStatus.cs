using System;

namespace RobotAction.Core
{
    public readonly struct PlayerEnergyInfo
    {
        public float MaxEnergy { get; }
        public float CurrentEnergy { get; }

        public PlayerEnergyInfo(float maxEnergy,float currentEnergy)
        {
            MaxEnergy = maxEnergy;
            CurrentEnergy = currentEnergy;
        }
    }

    public interface IPlayerStatus
    {
        public float MaxHealth { get; }
        public float CurrentHealth { get; }

        public float MaxEnergy { get; }
        public float CurrentEnergy { get; }

        public event Action<PlayerHealthInfo> OnHealthChanged;

        public event Action<PlayerEnergyInfo> OnEnergyChanged;
    }
}
