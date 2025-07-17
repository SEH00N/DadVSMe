using UnityEngine;
using UnityEngine.InputSystem;
using static DadVSMe.Inputs.InputActions;

namespace DadVSMe.Inputs
{
    public class PlayerInputReader : InputReaderBase, IPlayerActions
    {
        private InputActionMap inputActionMap = null;
        public override InputActionMap GetInputActionMap() => inputActionMap;

        public Vector2 MovementInput { get; private set; }

        public override void Initialize(InputActions inputActions)
        {
            PlayerActions playerActions = inputActions.Player;
            playerActions.SetCallbacks(this);
            inputActionMap = playerActions.Get();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                MovementInput = Vector2.zero;
                return;
            }

            MovementInput = context.ReadValue<Vector2>();
            MovementInput = MovementInput.normalized;
        }
    }
}
