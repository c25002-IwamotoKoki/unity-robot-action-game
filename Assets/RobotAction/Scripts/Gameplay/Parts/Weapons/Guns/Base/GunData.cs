using UnityEngine;

namespace RobotAction.Gameplay.Parts.Weapons.Guns
{
    [CreateAssetMenu(fileName = "GunData", menuName = "Scriptable Objects/GunData")]
    public abstract class GunData : ScriptableObject
    {
        [SerializeField] private string _name;

        public string Name => _name;
    }
}
