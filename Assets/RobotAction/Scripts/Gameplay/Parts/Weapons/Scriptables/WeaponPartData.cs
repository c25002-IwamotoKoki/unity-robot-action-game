using RobotAction.Core.RobotAssembly;
using UnityEngine;

namespace RobotAction.Gameplay.Parts.Weapons.Scriptables
{
    [CreateAssetMenu(fileName = "SO_weapon_part", menuName = "Robot/WeaponPartData")]
    public class WeaponPartData : ScriptableObject,IWeaponPartData
    {
        [SerializeField] private string _id;
        [SerializeField] private string _name;
        [SerializeField] private WeaponPartBase _partPrefab;
        [SerializeField] private Sprite _sprite;

        public string Id => _id;
        public string Name => _name;
        //IWeaponPartData実装による物コアはGameObjectじゃないと参照できないためCore用に公開
        public GameObject Prefab => _partPrefab.gameObject;
        public WeaponPartBase PartPrefab => _partPrefab;
        public Sprite Sprite => _sprite;
    }
}
