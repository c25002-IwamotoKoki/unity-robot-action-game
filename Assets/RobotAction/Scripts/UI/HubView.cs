using RobotAction.Core;
using UnityEngine;
using UnityEngine.UI;

namespace RobotAction.UI
{
    public class HubView : MonoBehaviour
    {
        [SerializeField] private Image _healthBar;
        [SerializeField] private Image _energyBar;

        public void SetHelth(PlayerHealthInfo healthInfo)
        {
            _healthBar.fillAmount = healthInfo.CurrentHealth / healthInfo.MaxHealth;
        }

        public void SetEenrgy(float maxEnergy, float currentEnergy)
        {
            _energyBar.fillAmount = currentEnergy / maxEnergy;
        }
    }
}
