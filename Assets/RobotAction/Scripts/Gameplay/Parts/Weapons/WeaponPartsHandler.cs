using RobotAction.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RobotAction.Gameplay.Parts.Weapons
{
    public class WeaponPartsHandler : PartsHandlerBase<IWeaponPart>, IWeaponStatus
    {
        public event Action<IWeaponPart> OnWeaponEquipped;
        public event Action<WeaponStatusInfo> OnWeaponStatusChanged;

        public int RemainingUseCount => CurrentPart?.RemainingUseCount ?? default;
        public int MaxUseCount => CurrentPart?.MaxUseCount ?? default;


        [SerializeField] private float _weaponSarchRange;
        [SerializeField, Min(1)] private int _maxWeaponSarch;
        [SerializeField] private LayerMask _sarchLayer;

        private Collider[] _detectedColliders;
        private List<IWeaponPart> _detectedWeaponParts;

        private void Awake()
        {
            _detectedColliders = new Collider[_maxWeaponSarch];
            _detectedWeaponParts = new(_maxWeaponSarch);
        }

        private void Start()
        {
            IWeaponPart weapon = GetComponentInChildren<IWeaponPart>();

            if (weapon != null)
            {
                Equip(weapon);
                OnWeaponEquipped?.Invoke(weapon);
            }
        }

        public bool TryPickUpNearlyWeapon()
        {
            if (TrySearchWeapon())
            {
                Equip(_detectedWeaponParts[0]);
                OnWeaponEquipped?.Invoke(_detectedWeaponParts[0]);
                return true;
            }

            return false;
        }

        protected override void OnEquip()
        {
            if(CurrentPart != null)
            {
                CurrentPart.OnWeaponStatusChanged += HandleWeaponStatusChanged;
            }

            var statusInfo = new WeaponStatusInfo(MaxUseCount, RemainingUseCount);
            OnWeaponStatusChanged?.Invoke(statusInfo);

            base.OnEquip();
        }

        protected override void OnUnequip()
        {
            if(CurrentPart != null)
            {
                CurrentPart.OnWeaponStatusChanged -= HandleWeaponStatusChanged;
            }

            var statusInfo = new WeaponStatusInfo(MaxUseCount,RemainingUseCount);
            OnWeaponStatusChanged?.Invoke(statusInfo);

            base.OnUnequip();
        }

        private void HandleWeaponStatusChanged(WeaponStatusInfo statusInfo)
        {
            OnWeaponStatusChanged?.Invoke(statusInfo);
        }

        private bool TrySearchWeapon()
        {
            _detectedWeaponParts.Clear();

            int searchCount = Physics.OverlapSphereNonAlloc(
                transform.position,
                _weaponSarchRange,
                _detectedColliders,
                _sarchLayer
            );
            if (searchCount != 0)
            {
                for (int i = 0; i < searchCount; i++)
                {
                    if (_detectedColliders[i].TryGetComponent(out IWeaponPart part))
                    {
                        if (!part.Owner)
                        {
                            _detectedWeaponParts.Add(part);
                        }
                    }
                }
            }

            return _detectedWeaponParts.Count > 0;
        }

        public void SetTarget(Vector3 position)
        {
            CurrentPart?.SetTarget(position);
        }

        public void Attack()
        {
            CurrentPart?.Attack();
        }
    }
}
