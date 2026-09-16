using UnityEngine;
using UnityEngine.Pool;

namespace RobotAction.Gameplay.ObjectPool
{
    public interface IPoolable<T> where T : Component
    {
        public void OnCreated(IObjectPool<T> ownerPool);

        public void OnGet();

        public void OnReturn();
    }
}
