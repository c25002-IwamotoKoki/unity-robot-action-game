using UnityEngine;
using System.Collections.Generic;

namespace RobotAction.Gameplay.Sensor
{
    public class TargetBuffer
    {
        public List<Transform> DetectedTargets { get; private set; } = new();
        private bool _isCapacitySet;

        public void SetCapacity(int capacity)
        {
            DetectedTargets = new List<Transform>(capacity);
            _isCapacitySet = true;
        }

        public void UpdateTargets(IReadOnlyList<Transform> targets)
        {
            if(!_isCapacitySet)
            {
                return;
            }

            DetectedTargets.Clear();

            for(int i = 0; i < targets.Count; i++)
            {
                DetectedTargets.Add(targets[i]);
            }
           
        }

    }
}
