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

        private void OnDisable()
        {
            if(CurrentPart != null)
            {
                CurrentPart.OnWeaponStatusChanged -= OnWeaponStatusChanged;
            }
        }

        public bool TryPickUpNearlyWeapon()
        {
            if (CurrentPart != null) return false;

            if (TrySearchWeapon())
            {
                Equip(_detectedWeaponParts[0]);
                OnWeaponEquipped?.Invoke(_detectedWeaponParts[0]);
                return true;
            }

            return false;
        }

        public override void Equip(IWeaponPart weapon)
        {
            if (weapon == null || weapon == CurrentPart) return;

            base.Equip(weapon);

            CurrentPart.OnWeaponStatusChanged += HandleWeaponStatusChanged;

            var statusInfo = new WeaponStatusInfo(MaxUseCount, RemainingUseCount);
            OnWeaponStatusChanged?.Invoke(statusInfo);
        }

        public override void Unequip()
        {
            if (CurrentPart == null) return;

            CurrentPart.OnWeaponStatusChanged -= HandleWeaponStatusChanged;

            //ëïîıâèúÇµÇΩÇΩÇﬂmax1,currentÇÕ0Ç≈ÉCÉxÉìÉgÇî≠çs
            //maxÇ™1Ç»ÇÃÇÕ0èúéZÇñhé~Ç∑ÇÈÇΩÇﬂÇ≈Ç∑
            var statusInfo = new WeaponStatusInfo(maxAmout: 1, remainingAmout: 0);
            OnWeaponStatusChanged?.Invoke(statusInfo);

            base.Unequip();
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
                        if (part.Owner == null && part != CurrentPart)
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
