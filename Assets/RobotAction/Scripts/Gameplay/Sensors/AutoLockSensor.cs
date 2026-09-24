using RobotAction.Gameplay.Enemy;
using System.Collections.Generic;
using UnityEngine;

namespace RobotAction.Gameplay.Sensors
{
    public class AutoLockSensor
    {
        private readonly Transform _owner;
        private readonly AutoLockSensorData _data;
        private readonly TargetBuffer _targetBuffer;
        private readonly Collider[] _detectedColliders;
        private readonly List<Transform> _detectedTargets;
        private float _sarchCoolTimer;

        public AutoLockSensor(Transform owner,AutoLockSensorData data, TargetBuffer buffer)
        {
            _owner = owner;
            _data = data;
            _targetBuffer = buffer;
            _targetBuffer.SetCapacity(_data.MaxSearch);
            _detectedColliders = new Collider[_data.MaxSearch];
            _detectedTargets = new List<Transform>(_data.MaxSearch);
        }

        public void Tick(float deltaTime, Vector3 sarchOrigin)
        {
            _sarchCoolTimer += deltaTime;

            if(_sarchCoolTimer >= _data.SearchCoolTime)
            {
                _sarchCoolTimer = 0;
                ExecuteSearch(sarchOrigin);
            }
        }

        private void ExecuteSearch(Vector3 sarchOrigin)
        {
            _detectedTargets.Clear();

            Vector3 sarchCentor = sarchOrigin + _data.SearchPositionOffset;
            int sarchCount = Physics.OverlapSphereNonAlloc(sarchCentor,
                                                           _data.SearchRange,
                                                           _detectedColliders);

            if (sarchCount != 0)
            {
                for (int i = 0; i < sarchCount; i++)
                {
                    if (_detectedColliders[i].TryGetComponent(out EnemyBase enemy))
                    {
                        _detectedTargets.Add(enemy.transform);
                    }
                }
            }

            for(int i = 0; i < _detectedTargets.Count-1; i++)
            {
                for(int j = i + 1; j < _detectedTargets.Count; j++)
                {
                    float sqrDistanceI = (_detectedTargets[i].position - _owner.position).sqrMagnitude;
                    float sqrDistanceJ = (_detectedTargets[j].position - _owner.position).sqrMagnitude;

                    //後ろ側の敵jが前側の敵iよりも近い位置にいるならスワップ
                    if(sqrDistanceJ < sqrDistanceI)
                    {
                        Transform temp = _detectedTargets[i];
                        _detectedTargets[i] = _detectedTargets[j];
                        _detectedTargets[j] =temp;
                    }

                }
            }

            _targetBuffer?.UpdateTargets(_detectedTargets);
        }
    }
}
