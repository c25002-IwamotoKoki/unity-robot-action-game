using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RobotAction.UI
{
    public sealed class WeaponPartHubView : MonoBehaviour
    {
        [SerializeField] private Image _rightWeaponDisplay;
        [SerializeField] private TextMeshProUGUI _rightWeaponNameDisplay;
        [SerializeField] private Image _leftWeaponDisplay;
        [SerializeField] private TextMeshProUGUI _leftWeaponNameDisplay;
        [SerializeField] private RectTransform _highlightDisplay;
        [SerializeField] private Color _submittedSlotColor;

        public void SetRightWeapon(Sprite sprite,string name)
        {
            _rightWeaponDisplay.sprite = sprite;
            _rightWeaponNameDisplay.text = name;
            _highlightDisplay.position = _rightWeaponNameDisplay.transform.position;
        }

        public void SetLeftWeapon(Sprite sprite , string name)
        {
            _leftWeaponDisplay.sprite = sprite;
            _leftWeaponNameDisplay.text = name;
            _highlightDisplay.position = _leftWeaponNameDisplay.transform.position;
        }

        public void SetRightWeaponHighlight()
        {
            _highlightDisplay.position = _rightWeaponNameDisplay.transform.position;
        }

        public void SetLeftWeaponHighlight()
        {
            _highlightDisplay.position = _leftWeaponNameDisplay.transform.position;
        }

        public void SetRightSubmitted()
        {
            _rightWeaponDisplay.color = _submittedSlotColor;
        }

        public void SetLeftSubmitted()
        {
            _leftWeaponDisplay.color = _submittedSlotColor;
        }

        public void SetRightUnsubmitted()
        {
            _rightWeaponDisplay.color = Color.white;
        }

        public void SetLeftUnsubmitted()
        {
            _leftWeaponDisplay.color = Color.white;
        }

    }
}
