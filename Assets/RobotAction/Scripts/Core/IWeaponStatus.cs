using System;

namespace RobotAction.Core
{
    public interface IWeaponStatus
    {
        public int MaxUseCount { get; }
        public int RemainingUseCount { get; }

        public event Action<WeaponStatusInfo> OnWeaponStatusChanged;
    }
}
