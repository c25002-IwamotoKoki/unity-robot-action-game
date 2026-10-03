using UnityEngine;

namespace RobotAction.Gameplay.Parts
{
    public class PartsHandlerBase<T> : MonoBehaviour where T : class, IPart
    {
        public T CurrentPart { get; private set; }

        public void Equip(T newPart)
        {
            CurrentPart?.Unequip();
            CurrentPart = newPart;
            CurrentPart.Equip(transform);
            OnEquip();
        }

        protected virtual void OnEquip()
        {

        }

        public void Unequip()
        {
            CurrentPart?.Unequip();
            CurrentPart = null;
            OnUnequip();
        }

        protected virtual void OnUnequip()
        {
            
        }

    }
}
