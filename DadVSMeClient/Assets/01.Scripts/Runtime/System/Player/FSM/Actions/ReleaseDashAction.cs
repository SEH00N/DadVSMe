using DadVSMe.Inputs;
using H00N.AI.FSM;

namespace DadVSMe.Players.FSM
{
    public class ReleaseDashAction : FSMAction
    {
        public override void EnterState()
        {
            base.EnterState();

            PlayerInputReader inputReader = InputManager.GetInput<PlayerInputReader>();
            inputReader.ReleaseDash();
        }
    }
}
