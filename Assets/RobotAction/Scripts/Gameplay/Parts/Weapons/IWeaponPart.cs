using RobotAction.Core;
using UnityEngine;

namespace RobotAction.Gameplay.Parts
{
    public interface IWeaponPart : IPart,IWeaponStatus
    {
        public float AttackRange { get; }

        public void SetTarget(Vector3 position);

        public void Attack();
    }
}
