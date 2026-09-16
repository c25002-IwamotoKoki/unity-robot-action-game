using UnityEngine;
using System.Collections.Generic;

namespace RobotAction.Gameplay.Parts.Weapons
{
    public class WeaponPartsHandler : PartsHandlerBase<IWeaponPart>
    {
        [SerializeField] private float _weaponSarchRange;
        [SerializeField,Min(1)] private int _maxWeaponSarch;
        [SerializeField] private LayerMask _sarchLayer;

        private Collider[] _detectedColliders;
        private List<IWeaponPart> _detectedWeaponParts;

        private void Awake()
        {
            _detectedColliders = new Collider[_maxWeaponSarch];
            _detectedWeaponParts = new(_maxWeaponSarch);
        }

        public bool TryPickUpNearlyWeapon()
        {
            if(TrySearchWeapon())
            {
                Equip(_detectedWeaponParts[0]);
                return true;
            }

            return false;
        }

        private bool TrySearchWeapon()
        {
            _detectedWeaponParts.Clear();

            int searchCount = Physics.OverlapSphereNonAlloc(transform.position,
                                                           _weaponSarchRange,
                                                           _detectedColliders,
                                                           _sarchLayer);

            if(searchCount != 0)
            {
                for(int i = 0; i < searchCount; i++)
                {
                    if(_detectedColliders[i].TryGetComponent(out IWeaponPart part))
                    {
                        if(!part.Owner)
                        {
                            _detectedWeaponParts.Add(part);
                        }
                    }
                }
            }

            return _detectedWeaponParts.Count > 0;
        }

        public void Attack()
        {
            CurrentPart?.Attack();
        }
    }
}
