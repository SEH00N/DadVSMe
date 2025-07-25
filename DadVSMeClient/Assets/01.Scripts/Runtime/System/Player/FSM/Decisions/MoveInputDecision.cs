using DadVSMe.Inputs;
using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe.Players.FSM
{
    public class MoveInputDecision : FSMDecision
    {
        public override bool MakeDecision()
        {
            PlayerInputReader inputReader = InputManager.GetInput<PlayerInputReader>();
            if(inputReader == null)
                return false;

            Vector2 movementInput = inputReader.MovementInput;
            return movementInput != Vector2.zero || movementInput.sqrMagnitude > 0.01f;
        }
    }
}
