using System;

namespace RobotAction.Core
{
    public interface IWeaponPartHandler
    {
        public int MaxUseCount { get; }
        public int RemainingUseCount { get; }

        public event Action OnWeaponStatusChanged;

        public event Action OnUnequipWeapon;
    }
}
