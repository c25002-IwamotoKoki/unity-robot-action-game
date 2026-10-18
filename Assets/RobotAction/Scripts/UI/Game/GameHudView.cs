using TMPro;
using UnityEngine;

namespace RobotAction.UI.Game
{
    public class GameHudView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _enemyKillProgressText;
        [SerializeField] private RectTransform _gameClearDisplay;
        [SerializeField] private RectTransform _defeatDisplay;
        [SerializeField] private Vector2 _activeGameClearTextPosition;
        [SerializeField] private Vector2 _activeDefeatDisplayPosition;

        public void SetKiilCount(int requiredCount,int currentCount)
        {
            _enemyKillProgressText.SetText("Target : {0}/{1}",currentCount,requiredCount);
        }

        public void SetGameClearText()
        {
            _gameClearDisplay.position = _activeGameClearTextPosition;
        }

        public void SetDefeatDisplay()
        {
            _defeatDisplay.position = _activeDefeatDisplayPosition;
        }
    }
}
