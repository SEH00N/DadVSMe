using DadVSMe.Inputs;
using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe.Players.FSM
{
    public class MoveInputDirectionDecision : FSMDecision
    {
        [Space(10f)]
        [SerializeField] bool checkHorizontal = true;
        [SerializeField, Range(-1, 1)] int directionX = 0;

        [Space(10f)]
        [SerializeField] bool checkVertical = true;
        [SerializeField, Range(-1, 1)] int directionY = 0;

        public override bool MakeDecision()
        {
            PlayerInputReader inputReader = InputManager.GetInput<PlayerInputReader>();
            if(inputReader == null)
                return false;

            Vector2 movementInput = inputReader.MovementInput;

            bool xMatch = checkHorizontal == false || Mathf.Approximately(movementInput.x, directionX);
            bool yMatch = checkVertical == false || Mathf.Approximately(movementInput.y, directionY);

            return xMatch && yMatch;
        }
    }
}
