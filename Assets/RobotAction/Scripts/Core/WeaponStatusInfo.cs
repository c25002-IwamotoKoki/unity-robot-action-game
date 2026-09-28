namespace RobotAction.Core
{
    public readonly struct WeaponStatusInfo
    {
        public int MaxUseCount { get; }
        public int RemainingUseCount { get; }

        public WeaponStatusInfo(int maxAmout,int remainingAmout)
        {
            MaxUseCount = maxAmout;
            RemainingUseCount = remainingAmout;
        }
    }
}
