using System;
using UnityEngine;

namespace RobotAction.Core.Sensor
{
    public interface IAutoLockSensorStatus
    {
        public Vector3 CurrentLockOnPosition { get; }

        public bool HasLockOnTarget { get; }
    }
}
