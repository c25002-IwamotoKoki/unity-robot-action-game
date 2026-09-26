using UnityEngine;

namespace RobotAction.Gameplay.Energy
{
    [CreateAssetMenu(fileName = "EnergyCoreData", menuName = "Scriptable Objects/EnergyCoreData")]
    public class EnergyCoreData : ScriptableObject
    {
        [SerializeField] private float _maxEnergy;
        [SerializeField] private float _recoveryRate;
        [SerializeField] private float _coolDownDuration;

        public float MaxEnergy => _maxEnergy;
        public float RecoveryRate => _recoveryRate;
        public float CoolDownDuration => _coolDownDuration;
    }
}
