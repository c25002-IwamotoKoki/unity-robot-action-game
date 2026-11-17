using UnityEngine;
using System.Collections.Generic;

namespace RobotAction.Gameplay.Sensors
{
    public class TargetBuffer
    {
        public Transform CurrentTarget { get; set; }


        private readonly List<Transform> _detectedTarget;
        private readonly HashSet<Transform> _targetSet;

        public int Count => _detectedTarget.Count;

        public bool HasTarget => _detectedTarget.Count > 0;

        public TargetBuffer(int capacity)
        {
            _detectedTarget = new(capacity);
            _targetSet = new(capacity);
        }

        public void UpdateTargets(IReadOnlyList<Transform> targets)
        {
            _detectedTarget.Clear();
            _targetSet.Clear();

            for(int i = 0; i < targets.Count; i++)
            {
                _detectedTarget.Add(targets[i]);
                _targetSet.Add(targets[i]);
            }       
        }

        public Transform GetByIndex(int index)
        {
            return _detectedTarget[index];
        }

        public bool Contains(Transform target)
        {
            return _targetSet.Contains(target);
        }
    }
}
