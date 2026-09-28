using UnityEngine;
using UnityEngine.UI;

namespace RobotAction.UI
{
    public class HubView : MonoBehaviour
    {
        [SerializeField] private Image _healthBar;
        [SerializeField] private Image _energyBar;

        [SerializeField] private Image _rightWeaponBar;
        [SerializeField] private Image _leftWeaponBar;

        public void SetHelth(float maxHealth,float currentHealth)
        {
            _healthBar.fillAmount = currentHealth / maxHealth;
        }

        public void SetEenrgy(float maxEnergy, float currentEnergy)
        {
            _energyBar.fillAmount = currentEnergy / maxEnergy;
        }

        public void SetRightWeapon(int maxUseCount, int remainingUseCount)
        {
            _rightWeaponBar.fillAmount = (float)remainingUseCount / maxUseCount;
        }

        public void SetLeftWeapon(int maxUseCount, int remainingUseCount)
        {
            _leftWeaponBar.fillAmount = (float)remainingUseCount / maxUseCount;
        }

    }
}
