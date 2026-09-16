using System;
using UnityEngine;

namespace RobotAction.Gameplay.Parts
{
    public interface IPart
    {
        public Transform Owner { get; }

        public void Equip(Transform owner);

        public void Unequip();
    }
}
