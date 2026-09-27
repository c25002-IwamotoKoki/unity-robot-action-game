using System;

namespace RobotAction.Core
{
    public readonly struct PlayerHealthInfo
    {
        public float MaxHealth { get; }
        public float CurrentHealth { get; }

        public PlayerHealthInfo(float maxHealth, float currentHealth)
        {
            MaxHealth = maxHealth;
            CurrentHealth = currentHealth;
        }
    }

    public interface IPlayerStatus
    {
        public float CurrentHealth { get; }
        public float MaxHealth { get; }

        public event Action<PlayerHealthInfo> OnHealthChanged;
    }
}
