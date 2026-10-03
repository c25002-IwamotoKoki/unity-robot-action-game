using RobotAction.Gameplay.Player;
using RobotAction.UI;
using UnityEngine;

namespace RobotAction.Root.Battle
{
    public sealed class BattleEntryPoint : MonoBehaviour
    {
        [SerializeField] private PlayerController _player;
        [SerializeField] private PlayerStatusHudPresenter _hudPresentor;
        [SerializeField] private PlayerStatusHudView _hudView;

        private void Start()
        {
            _hudPresentor.Initialize(
                _player,
                _player.RightWeaponPartsHandler,
                _player.LeftWeaponPartsHandler
            );
        }
    }
}
