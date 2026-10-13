using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RobotAction.UI
{
    public class PlayerStatusHudView : MonoBehaviour
    {
        [SerializeField] private Image _healthBar;
        [SerializeField] private TextMeshProUGUI _healthBarText;
        [SerializeField] private Image _energyBar;
        [SerializeField] private TextMeshProUGUI _energyBarText;
        [SerializeField] private Image _rightWeaponBar;
        [SerializeField] private TextMeshProUGUI _rightWeaponBarText;
        [SerializeField] private Image _leftWeaponBar;
        [SerializeField] private TextMeshProUGUI _leftWeaponBarText;

        public void SetHelth(float maxHealth,float currentHealth)
        {
            _healthBar.fillAmount = currentHealth / maxHealth;
            //UIに小数点以下を表示させないためintにキャスト
            _healthBarText.SetText("HP:{0}/{1}",(int)currentHealth,maxHealth);
        }

        public void SetEenrgy(float maxEnergy, float currentEnergy)
        {
            _energyBar.fillAmount = currentEnergy / maxEnergy;
            //UIに小数点以下を表示させないためintにキャスト
            _energyBarText.SetText("Energy:{0}/{1}",(int)currentEnergy,maxEnergy);
        }

        public void SetRightWeapon(int maxUseCount, int remainingUseCount)
        {
            _rightWeaponBar.fillAmount = (float)remainingUseCount / maxUseCount;
            _rightWeaponBarText.SetText("RightWeapon:{0}/{1}",remainingUseCount,maxUseCount);
        }

        public void SetLeftWeapon(int maxUseCount, int remainingUseCount)
        {
            _leftWeaponBar.fillAmount = (float)remainingUseCount / maxUseCount;
            _leftWeaponBarText.SetText("LeftWeapon:{0}/{1}", remainingUseCount, maxUseCount);
        }

    }
}
