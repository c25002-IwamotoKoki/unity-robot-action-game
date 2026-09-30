using RobotAction.Core.RobotAssembly;
using System.Collections.Generic;
using UnityEngine;

namespace RobotAction.Gameplay.Parts.Weapons
{
    [CreateAssetMenu(fileName = "WeaponPartsCatalog", menuName = "Robot/WeaponPartsCatalog")]
    public sealed class WeaponPartCatalog : ScriptableObject
    {
        [SerializeField] private WeaponPartData[] _data;

        public IReadOnlyList<WeaponPartData> Data => _data;

        private Dictionary<string, WeaponPartData> _partsById;

        public void SetupDictionary()
        {
            if (_data == null || _data.Length == 0)
            {
                Debug.LogWarning("[WeaponPartsCatalog]何もデータがセットされていません");
                return;
            }

            _partsById = new(_data.Length);

            foreach(var data in _data)
            {
                if(data == null || string.IsNullOrEmpty(data.Id))
                {
                    Debug.LogWarning("[WeaponPartsCatalog]不適切なIdまたはデータが含まれています");
                    continue;
                }

                if(!_partsById.TryAdd(data.Id,data))
                {
                    Debug.LogWarning($"[WeaponPartsCatalog]Id:{data.Id}が重複しています");
                }
            }
        }

        public IWeaponPartData GetById(string id)
        {
            if(string.IsNullOrEmpty(id)) return null;

            if (_partsById.TryGetValue(id, out var data))
            {
                return data;
            }

            Debug.LogWarning($"[WeaponPartsCatalog]Id:{id}のWeaponPartが見つかりませんでした");

            return null;
        }
    }
}
