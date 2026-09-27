using UnityEngine;
using UnityEngine.UI;

namespace RobotAction.UI
{
    public class HubView : MonoBehaviour
    {
        [SerializeField] private Image _healthBar;
        [SerializeField] private Image _energyBar;

        public void SetHelth(float maxHealth,float currentHealth)
        {
            _healthBar.fillAmount = currentHealth / maxHealth;
        }

        public void SetEenrgy(float maxEnergy, float currentEnergy)
        {
            _energyBar.fillAmount = currentEnergy / maxEnergy;
        }
    }
}
