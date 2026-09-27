using RobotAction.Gameplay.Player;
using RobotAction.UI;
using UnityEngine;

namespace RobotAction.Root
{
    public sealed class GameEntryPoint : MonoBehaviour
    {
        [SerializeField] private PlayerController _player;
        [SerializeField] private HubPresenter _hubPresentor;
        [SerializeField] private HubView _hubView;

        public void Awake()
        {
            _hubPresentor.Initialize(_player);
        }
    }
}
