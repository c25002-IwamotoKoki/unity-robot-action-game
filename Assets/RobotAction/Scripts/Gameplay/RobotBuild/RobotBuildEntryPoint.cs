using RobotAction.Core.RobotAssembly;
using RobotAction.Core.Scene;
using RobotAction.Gameplay.Parts.Weapons;
using RobotAction.Gameplay.Scene;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RobotAction.Gameplay.RobotBuild
{
    public class RobotBuildEntryPoint : MonoBehaviour
    {
        [SerializeField] private WeaponPartSelector _weaponPartSelector;

        private void OnEnable()
        {
            _weaponPartSelector.OnAllSubmitted += HandleAllWeaponSubmitted;
        }

        private void OnDisable()
        {
            _weaponPartSelector.OnAllSubmitted -= HandleAllWeaponSubmitted;
        }

        private void HandleAllWeaponSubmitted(RobotAssemblyInfo info)
        {
            CrossSceneRobotSession.CurrentAssemblyInfo = info;
            SceneManager.LoadScene(SceneNames.Battle);
        }
    }
}
