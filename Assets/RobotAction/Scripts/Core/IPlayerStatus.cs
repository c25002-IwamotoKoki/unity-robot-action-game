using System;

namespace RobotAction.Core
{
    public interface IPlayerStatus
    {
        public float MaxHealth { get; }
        public float CurrentHealth { get; }

        public float MaxEnergy { get; }
        public float CurrentEnergy { get; }

        public event Action OnHealthChanged;

        public event Action OnEnergyChanged;

        public event Action OnDied;
    }
}
