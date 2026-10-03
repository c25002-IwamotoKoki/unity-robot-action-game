using RobotAction.Core.RobotAssembly;
using RobotAction.Gameplay.Parts;
using RobotAction.Gameplay.Parts.Weapons;
using RobotAction.Gameplay.Player;
using RobotAction.Gameplay.Scene;
using System;
using UnityEngine;

namespace RobotAction.Gameplay.RobotBuild
{
    public sealed class RobotBuilder : MonoBehaviour
    {
        [SerializeField] private WeaponPartCatalog _weaponCatalog;
        [SerializeField] private PlayerController _player;

        private void Start()
        {
            var assemblyInfo = CrossSceneRobotSession.CurrentAssemblyInfo;

            EquipWeapon(assemblyInfo.RightWeaponPartId,_player.RightWeaponPartsHandler);
            EquipWeapon(assemblyInfo.LeftWeaponPartId, _player.LeftWeaponPartsHandler);
        }

        private void EquipWeapon(string partId,WeaponPartsHandler handler)
        {
            if(string.IsNullOrEmpty(partId))
            {
                throw new System.ArgumentNullException(nameof(partId),partId);
            }

            IWeaponPartData weaponPartData = _weaponCatalog.GetById(partId);
            if(weaponPartData == null)
            {
                throw new InvalidOperationException($"[RobotBuilder] WeaponPartCatalog“à‚ÌId:{partId}‚ÌWeaponPartData‚ªnull‚Å‚·");
            }

            GameObject instance = Instantiate(weaponPartData.Prefab);
            if(!instance.TryGetComponent(out IWeaponPart weapon))
            {
                throw new MissingComponentException($"[RobotBuilder]Id:{weaponPartData.Prefab.name}‚ÌPrefab‚É{nameof(IWeaponPart)}‚ªŒ©‚Â‚©‚è‚Ü‚¹‚ñ");
            }

            handler.Equip(weapon);
        }
    }
}
