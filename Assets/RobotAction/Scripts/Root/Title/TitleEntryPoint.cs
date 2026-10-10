using RobotAction.Core.Scene;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace RobotAction.Root.Title
{
    public class TitleEntryPoint : MonoBehaviour
    {
        [SerializeField] private InputActionReference _startAction;
        [SerializeField] private int _frameRate = 60;

        private bool _isLoading;

        private void Awake()
        {
            Application.targetFrameRate = _frameRate;
        }

        private void OnEnable()
        {
            _startAction.action.started += HandleStartButtonStarted;
            _startAction.action.Enable();
        }

        private void OnDisable()
        {
            _startAction.action.started -= HandleStartButtonStarted;
            _startAction.action.Disable();
        }

        public void OnStartButtonClicked()
        {
            LoadRobotBuildScene();
        }

        private void HandleStartButtonStarted(InputAction.CallbackContext context)
        {
            LoadRobotBuildScene();
        }

        private void LoadRobotBuildScene()
        {
            if (_isLoading) return;

            _isLoading = true;

            SceneManager.LoadScene(SceneNames.RobotBuild);
        }
    }
}
