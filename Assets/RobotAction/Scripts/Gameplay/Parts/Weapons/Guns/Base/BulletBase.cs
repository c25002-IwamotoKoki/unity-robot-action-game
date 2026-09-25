using UnityEngine;
using UnityEngine.Pool;
using RobotAction.Gameplay.ObjectPool;

namespace RobotAction.Gameplay.Parts.Weapons.Guns
{
    public abstract class BulletBase : MonoBehaviour,IPoolable<BulletBase>
    {
        protected IObjectPool<BulletBase> OwnerPool { get; private set; }

        private bool _isReturned;

        public virtual void OnCreated(IObjectPool<BulletBase> ownerPool)
        {
            OwnerPool = ownerPool;
        }

        public virtual void OnGet()
        {
            _isReturned = false;
        }

        public virtual void OnReturn()
        {
            //2èdï‘ãpñhé~óp
            if(_isReturned)
            {
                return;
            }

            _isReturned = true;
        }

        public abstract void Fire(in BulletContext context);
    }
}
