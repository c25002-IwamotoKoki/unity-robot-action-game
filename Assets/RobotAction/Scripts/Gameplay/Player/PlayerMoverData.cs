using UnityEngine;

namespace RobotAction.Gameplay.Player
{
    [CreateAssetMenu(fileName = "PlayerMoverData", menuName = "Scriptable Objects/PlayerMoverData")]
    public class PlayerMoverData : ScriptableObject,IPlayerMoverData
    {
        [Header("Basic Movement")]
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _defaultMaxSpeed;
        [SerializeField] private float _moveEnergyCost;

        [Space(10)]

        [Header("Boost Movement")]
        [SerializeField] private float _boostSpeed;
        [SerializeField] private float _boostEnergyCost;
        [SerializeField] private float _boostMaxSpeed;

        [Space(10)]

        [Header("Phy")]
        [SerializeField] private float _maxSpeedDeceleration;

        public float MoveSpeed => _moveSpeed;
        public float DefaultMaxSpeed => _defaultMaxSpeed;
        public float MoveEnergyCost => _moveEnergyCost;
        public float BoostSpeed => _boostSpeed;
        public float BoostEnergyCost => _boostEnergyCost;
        public float BoostMaxSpeed => _boostMaxSpeed;
        public float MaxSpeedDeceleration => _maxSpeedDeceleration;
    }
}
