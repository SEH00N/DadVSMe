using UnityEngine.InputSystem;

namespace DadVSMe.Inputs
{
    public abstract class InputReaderBase
    {
        public virtual void Initialize(InputActions inputActions) { }
        public virtual void Release() { }

        public abstract InputActionMap GetInputActionMap();

        public virtual void Update() { }
    }
}
