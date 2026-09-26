using UnityEngine;
using UnityEngine.Pool;
using RobotAction.Gameplay.ObjectPool;

namespace RobotAction.Gameplay.Parts.Weapons.Guns
{
    public abstract class BulletBase : MonoBehaviour,IPoolable<BulletBase>
    {
        protected IObjectPool<BulletBase> OwnerPool { get; private set; }

        protected bool _isReleased;

        public virtual void OnCreated(IObjectPool<BulletBase> ownerPool)
        {
            OwnerPool = ownerPool;
        }

        public virtual void OnGet()
        {
            _isReleased = false;
        }

        public virtual void OnReturn()
        {
            //2èdï‘ãpñhé~óp
            if(_isReleased)
            {
                return;
            }

            _isReleased = true;
        }

        public abstract void Fire(in BulletContext context);
    }
}
