using RobotAction.Core;
using System;

namespace RobotAction.Gameplay.Energy
{
    public class EnergyCore
    {
        public float MaxEnergy { get; }
        public float CurrentEnergy { get; private set; }

        public event Action<PlayerEnergyInfo> OnEnergyChanged;

        private readonly float _recoveryRate;
        private readonly float _coolDownDuration;

        private float _coolTimer;

        public EnergyCore(EnergyCoreData data)
        {
            MaxEnergy = data.MaxEnergy;
            CurrentEnergy = MaxEnergy;
            _recoveryRate = data.RecoveryRate;
            _coolDownDuration = data.CoolDownDuration;
        }

        public void Tick(float deltaTime)
        {
            if (_coolTimer > 0f)
            {
                _coolTimer -= deltaTime;
                return;
            }

            if(CurrentEnergy < MaxEnergy)
            {
                CurrentEnergy = MathF.Min(MaxEnergy,CurrentEnergy + _recoveryRate * deltaTime);

                var energyInfo = new PlayerEnergyInfo(MaxEnergy, CurrentEnergy);
                OnEnergyChanged?.Invoke(energyInfo);
            }
        }

        public bool TryCosume(float value)
        {
            if (CurrentEnergy < value) return false;

            var energyInfo = new PlayerEnergyInfo(MaxEnergy,CurrentEnergy);
            OnEnergyChanged?.Invoke(energyInfo);

            CurrentEnergy -= value;
            _coolTimer = _coolDownDuration;

            return true;
        }
    }
}
