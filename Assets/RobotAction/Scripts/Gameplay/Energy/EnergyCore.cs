using System;

namespace RobotAction.Gameplay.Energy
{
    public class EnergyCore
    {
        public float MaxEnergy { get; }

        private readonly float _recoveryRate;
        private readonly float _coolDownDuration;

        private float _currentEnergy;
        private float _coolTimer;

        public EnergyCore(EnergyCoreData data)
        {
            MaxEnergy = data.MaxEnergy;
            _currentEnergy = MaxEnergy;
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

            if(_currentEnergy < MaxEnergy)
            {
                _currentEnergy = MathF.Min(MaxEnergy, _currentEnergy + _recoveryRate * deltaTime);
            }
        }

        public bool TryCosume(float value)
        {
            if (_currentEnergy < value) return false;

            _currentEnergy -= value;
            _coolTimer = _coolDownDuration;

            return true;
        }

    }
}
