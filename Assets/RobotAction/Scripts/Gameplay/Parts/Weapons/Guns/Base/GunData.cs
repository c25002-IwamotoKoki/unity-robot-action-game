using UnityEngine;

namespace RobotAction.Gameplay.Parts.Weapons.Guns
{
    public abstract class GunData : ScriptableObject
    {
        [SerializeField] private string _name;

        public string Name => _name;
    }
}
