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

        public TargetBuffer(int capacity)
        {
            DetectedTargets = new(capacity);
            TargetSet = new(capacity);
        }

        public void UpdateTargets(IReadOnlyList<Transform> targets)
        {
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
