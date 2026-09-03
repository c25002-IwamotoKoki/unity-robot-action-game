using UnityEngine;

namespace RobotAction.Core
{
    public class GameController : MonoBehaviour
    {
        private void Awake()
        {
            Application.targetFrameRate = 60;
        }
    }
}
