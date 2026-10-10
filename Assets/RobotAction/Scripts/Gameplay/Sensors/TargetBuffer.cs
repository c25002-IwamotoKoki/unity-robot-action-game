using UnityEngine;
using System.Collections.Generic;

namespace RobotAction.Gameplay.Sensors
{
    public class TargetBuffer
    {
        public Transform CurrentTarget { get; set; }

        public List<Transform> DetectedTargets { get; private set; } = new();
        public HashSet<Transform> TargetSet { get; private set; }

        public bool HasTarget => DetectedTargets.Count > 0;

        private bool _isCapacitySet;

        public TargetBuffer()
        {
            DetectedTargets = new();
            TargetSet = new();
        }

        public void SetCapacity(int capacity)
        {
            DetectedTargets = new List<Transform>(capacity);
            TargetSet = new HashSet<Transform>(capacity);
            _isCapacitySet = true;
        }

        public void UpdateTargets(IReadOnlyList<Transform> targets)
        {
            if(!_isCapacitySet)
            {
                return;
            }

            DetectedTargets.Clear();
            TargetSet.Clear();

            for(int i = 0; i < targets.Count; i++)
            {
                DetectedTargets.Add(targets[i]);
                TargetSet.Add(targets[i]);
            }       
        }
    }
}
