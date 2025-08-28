using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static DadVSMe.Inputs.InputActions;

namespace DadVSMe.Inputs
{
    public class PlayerInputReader : InputReaderBase, IPlayerActions
    {
        private const float DASH_INPUT_THRESHOLD = 0.35f;

        private InputActionMap inputActionMap = null;
        public override InputActionMap GetInputActionMap() => inputActionMap;
        
        public Vector2 MovementInput { get; private set; }

        private DoublePress leftDashDoublePress = new DoublePress(DASH_INPUT_THRESHOLD);
        private DoublePress rightDashDoublePress = new DoublePress(DASH_INPUT_THRESHOLD);
        public bool IsDashed { get; private set; }

        private bool attack1PhaseBufferFlag = false;
        private InputActionPhase attack1PhaseBuffer = InputActionPhase.Disabled;

        private bool attack2PhaseBufferFlag = false;
        private InputActionPhase attack2PhaseBuffer = InputActionPhase.Disabled;

        public event Action onPressPause = null;

        public override void Initialize(InputActions inputActions)
        {
            PlayerActions playerActions = inputActions.Player;
            playerActions.SetCallbacks(this);
            inputActionMap = playerActions.Get();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            #if UNITY_EDITOR
            // Debug.Log($"[PlayerInputReader] OnMove Phase : {context.phase}");
            #endif

            if (context.canceled)
            {
                MovementInput = Vector2.zero;
                IsDashed = false;
                return;
            }

            Vector2 movementInput = context.ReadValue<Vector2>().normalized;
            MovementInput = movementInput;
        }

        public void OnLeftDash(InputAction.CallbackContext context)
        {
            #if UNITY_EDITOR
            // Debug.Log($"[PlayerInputReader] OnLeftDash Phase : {context.phase}");
            #endif

            if(context.started == false)
                return;

            IsDashed = leftDashDoublePress.Press();
        }

        public void OnRightDash(InputAction.CallbackContext context)
        {
            #if UNITY_EDITOR
            // Debug.Log($"[PlayerInputReader] OnRightDash Phase : {context.phase}");
            #endif

            if(context.started == false)
                return;

            IsDashed = rightDashDoublePress.Press();
        }

        public void ReleaseDash()
        {
            IsDashed = false;
        }

        public bool GetAttack1Down() => (attack1PhaseBuffer == InputActionPhase.Performed) && (attack1PhaseBufferFlag == true);
        public bool GetAttack1Press() => (attack1PhaseBuffer == InputActionPhase.Performed) && (attack1PhaseBufferFlag == false);
        public bool GetAttack1Up() => (attack1PhaseBuffer == InputActionPhase.Canceled) && (attack1PhaseBufferFlag == true);
        public void OnAttack1(InputAction.CallbackContext context)
        {
            #if UNITY_EDITOR
            // Debug.Log($"[PlayerInputReader] OnAttack1 Phase : {context.phase}");s
            #endif

            attack1PhaseBuffer = context.phase;
            attack1PhaseBufferFlag = true;
        }

        public bool GetAttack2Down() => (attack2PhaseBuffer == InputActionPhase.Performed) && (attack2PhaseBufferFlag == true);
        public bool GetAttack2Press() => (attack2PhaseBuffer == InputActionPhase.Performed) && (attack2PhaseBufferFlag == false);
        public bool GetAttack2Up() => (attack2PhaseBuffer == InputActionPhase.Canceled) && (attack2PhaseBufferFlag == true);
        public void OnAttack2(InputAction.CallbackContext context)
        {
            #if UNITY_EDITOR
            // Debug.Log($"[PlayerInputReader] OnAttack2 Phase : {context.phase}");
            #endif

            attack2PhaseBuffer = context.phase;
            attack2PhaseBufferFlag = true;
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            if(context.performed)
            {
                onPressPause?.Invoke();
            }
        }

        public override void Update()
        {
            if(attack1PhaseBufferFlag == false && attack2PhaseBufferFlag == false)
                return;

            EndOfFrameRoutine().Forget();
        }

        private async UniTask EndOfFrameRoutine()
        {
            await UniTask.WaitForEndOfFrame();

            attack1PhaseBufferFlag = false;
            attack2PhaseBufferFlag = false;
        }
    }
}
