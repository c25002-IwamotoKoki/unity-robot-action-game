using RobotAction.Gameplay.ObjectPool;
using UnityEngine;

namespace RobotAction.Gameplay.Enemy
{
    public class EnemyPool : ComponentPoolBase<EnemyBase>
    {
        private bool _isSetCpacity;
        private bool _isInitialize;
        public bool CanSpawn  => _isInitialize && GetCountAll() <= _defaultCapacity;

        public EnemyBase Spawn()
        {
            return Pool.Get();
        }

        public void SetCapacity(int capacity)
        {
            _defaultCapacity = capacity;

            //defaultCapacity‚Æ“¯‚¶”‚¾‚Æ‹““®‚ª‚¨‚©‚µ‚­‚È‚é‚½‚ß1‚Â‘½‚ß‚ÉŠm•Û
            _maxCapacity = capacity + 1;

            _isSetCpacity = true;
        }

        public void PrewarmPool()
        {
            if (!_isSetCpacity) return;

            PrewarmPoolFlow();

            _isInitialize = true;
        }

    }
}
