using UnityEngine;
using UnityEngine.InputSystem;

namespace DadVSMe.Inputs
{
    public abstract class InputReaderBase : ScriptableObject
    {
        public virtual void Initialize(InputActions inputActions) { }
        public virtual void Release() { }

        public abstract InputActionMap GetInputActionMap();
    }
}
