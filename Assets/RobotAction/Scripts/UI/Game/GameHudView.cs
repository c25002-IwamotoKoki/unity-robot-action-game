using TMPro;
using UnityEngine;

namespace RobotAction.UI.Game
{
    public class GameHudView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _enemyKillProgressText;
        [SerializeField] private RectTransform _gameClearText;
        [SerializeField] private RectTransform _defeatText;
        [SerializeField] private Vector2 _activeGameClearTextPosition;
        [SerializeField] private Vector2 _activeDefeatTextPosition;

        public void SetKiilCount(int requiredCount,int currentCount)
        {
            _enemyKillProgressText.SetText("RequiredDefatEnemy : {0}/{1}",currentCount,requiredCount);
        }

        public void SetGameClearText()
        {
            _gameClearText.position = _activeGameClearTextPosition;
        }

        public void SetDefeatText()
        {
            _defeatText.position = _activeDefeatTextPosition;
        }
    }
}
