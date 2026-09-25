using UnityEngine;

namespace RobotAction.Gameplay.Scriptables
{
    [CreateAssetMenu(fileName = "GunData", menuName = "Scriptable Objects/EnergyGunData")]
    public class EnergyGunData : GunData
    {
        [SerializeField] private float _fireRate;
        [SerializeField] private float _damage;
        [SerializeField] private float _maxRange;
        [SerializeField] private float _beamExtendSpeed;
        [SerializeField] private LayerMask _hitLayer;
        
        public float FireRate => _fireRate;
        public float Damage => _damage;
        public float MaxRange => _maxRange;
        public float BeamExtendSpeed => _beamExtendSpeed;
        public LayerMask HitLayer => _hitLayer;
    }
}
