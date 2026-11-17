using RobotAction.Core;
using RobotAction.Core.Audio;
using UnityEngine;

namespace RobotAction.Gameplay.Parts
{
    public interface IWeaponPart : IPart,IWeaponStatus
    {
        public float AttackRange { get; }
        public IAudioService AudioService { get; }

        public void SetTarget(Vector3 position);

        public void ResetTarget();

        public void Attack();

        public void SetAttackTargetLayer(LayerMask targetLayer);

        public void SetAudioService(IAudioService audioService);
    }
}
