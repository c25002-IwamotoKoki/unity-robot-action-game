using RobotAction.Core.RobotAssembly;
using UnityEngine;

namespace RobotAction.Gameplay.Parts.Weapons
{
    [CreateAssetMenu(fileName = "SO_weapon_part", menuName = "Robot/WeaponPartData")]
    public class WeaponPartData : ScriptableObject,IWeaponPartData
    {
        [SerializeField] private string _id;
        [SerializeField] private string _name;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private Sprite _sprite;

        public string Id => _id;
        public string Name => _name;
        public GameObject Prefab => _prefab;
        public Sprite Sprite => _sprite;
    }
}
