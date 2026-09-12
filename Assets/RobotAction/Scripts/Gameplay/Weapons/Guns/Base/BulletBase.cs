using RobotAction.Gameplay.Interfaces;
using UnityEngine;
using UnityEngine.Pool;

namespace RobotAction.Gameplay.Weapons.Guns
{
    public abstract class BulletBase : MonoBehaviour,IPoolable<BulletBase>
    {
        protected IObjectPool<BulletBase> OwnerPool { get; private set; }

        public virtual void OnCreated(IObjectPool<BulletBase> ownerPool)
        {
            OwnerPool = ownerPool;
        }

        public virtual void OnGet()
        {

        }

        public virtual void OnReturn()
        {

        }

        public abstract void Shoot(in BulletContext context);
    }
}
