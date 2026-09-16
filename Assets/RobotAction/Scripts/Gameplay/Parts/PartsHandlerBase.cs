using UnityEngine;

namespace RobotAction.Gameplay.Parts
{
    public class PartsHandlerBase<T> : MonoBehaviour where T : class, IPart
    {
        public T CurrentPart { get; private set; }

        protected void Equip(T newPart)
        {
            CurrentPart?.Unequip();
            CurrentPart = newPart;
            CurrentPart.Equip(transform);
        }

        public void Unequip()
        {
            CurrentPart?.Unequip();
            CurrentPart = null;
        }
    }
}
