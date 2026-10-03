namespace RobotAction.Core.RobotAssembly
{
    public readonly struct RobotAssemblyInfo
    {
        public string RightWeaponPartId { get; }
        public string LeftWeaponPartId { get; }

        public RobotAssemblyInfo(string rightWeaponPartId,string leftWeaponPartId)
        {
            RightWeaponPartId = rightWeaponPartId;
            LeftWeaponPartId = leftWeaponPartId;
        }
    }
}
