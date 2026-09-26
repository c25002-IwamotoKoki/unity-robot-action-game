using UnityEngine;

namespace RobotAction.Gameplay.Parts.Weapons.Guns
{
    [CreateAssetMenu(fileName = "GunData", menuName = "Scriptable Objects/EnergyGunData")]
    public class EnergyGunData : GunData
    {
        [SerializeField] private float _fireRate;
        [SerializeField] private float _damage;
        [SerializeField] private float _maxRange;
        [SerializeField] private float _beamExtendSpeed;
        [SerializeField,Min(1)] private int _maxHitCount;
        [SerializeField] private LayerMask _hitLayer;
        
        public float FireRate => _fireRate;
        public float Damage => _damage;
        public float MaxRange => _maxRange;
        public float BeamExtendSpeed => _beamExtendSpeed;
        public int MaxHitCount => _maxHitCount;
        public LayerMask HitLayer => _hitLayer;
    }
}
