using UnityEngine;

namespace RobotAction.Core.RobotAssembly
{
    public interface IWeaponPartData
    {
        public string Id { get; }
        public string Name { get; }
        public GameObject Prefab { get; }
        public Sprite Sprite { get; }
    }
}
