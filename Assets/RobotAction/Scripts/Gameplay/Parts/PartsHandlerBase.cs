using UnityEngine;

namespace RobotAction.Gameplay.Parts
{
    public class PartsHandlerBase<T> : MonoBehaviour where T : class, IPart
    {
        public T CurrentPart { get; private set; }

        public virtual void Equip(T newPart)
        {
            CurrentPart?.Unequip();
            CurrentPart = newPart;
            CurrentPart.Equip(transform);
        }

        public virtual void Unequip()
        {
            CurrentPart?.Unequip();
            CurrentPart = null;
        }

    }
}
