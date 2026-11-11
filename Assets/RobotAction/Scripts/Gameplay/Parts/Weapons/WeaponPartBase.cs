using RobotAction.Core.Audio;
using System;
using UnityEngine;

namespace RobotAction.Gameplay.Parts.Weapons
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public abstract class WeaponPartBase : MonoBehaviour,IWeaponPart
    {
        public event Action OnWeaponStatusChanged;

        public Transform Owner { get; private set; }
        public IAudioService AudioService { get; private set; }
        public float AttackRange { get; protected set; }
        public int RemainingUseCount { get; protected set; }
        public abstract int MaxUseCount { get; }


        //åpè≥êÊÇ©ÇÁÇÃà”ê}ÇµÇ»Ç¢ílÇÃïœçXÇñhÇÆÇΩÇﬂÅAåpè≥êÊÇÃÉNÉâÉXÇ…ëŒÇµÇƒì«Ç›éÊÇËêÍópÇ≈ï€éù
        protected LayerMask AttackTargetLayer { get; private set; }

        private Rigidbody _rigidbody;
        private Collider _collider;

        private void Awake()
        {
            TryGetComponent(out _rigidbody);
            TryGetComponent(out _collider);

            OnAwake();
        }

        protected virtual void OnAwake()
        {

        }

        public virtual void SetTarget(Vector3 target)
        {

        }

        public virtual void ResetTarget()
        {

        }

        public virtual void SetAudioSurvice(IAudioService audioService)
        {
            AudioService = audioService;
        }

        public virtual void SetAttackTargetLayer(LayerMask layerMask)
        {
            AttackTargetLayer = layerMask;
        }

        public abstract void Attack();

        public virtual void Equip(Transform owner)
        {
            Owner = owner;
            _rigidbody.isKinematic = true;
            _collider.enabled = false;
            transform.SetParent(Owner);
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }

        public virtual void Unequip()
        {
            Owner = null;
            _rigidbody.isKinematic = false;
            _collider.enabled = true;
            transform.SetParent(null);
        }

        protected void InvokeOnWeaponStatusChangedEvent()
        {
            OnWeaponStatusChanged?.Invoke();
        }

        protected void PlayShotSE(string audioId)
        {
            AudioService?.PlaySE(audioId);
        }
    }
}
