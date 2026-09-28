using UnityEngine;

namespace RobotAction.Gameplay.Parts.Weapons.Guns
{
    public abstract class GunData : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField,Min(1)] private int _maxUseCount = 1;

        public string Name => _name;
        public int MaxAmmo => _maxUseCount;
    }
}
