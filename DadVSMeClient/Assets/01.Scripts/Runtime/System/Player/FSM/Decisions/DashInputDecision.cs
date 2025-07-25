using DadVSMe.Inputs;
using H00N.AI.FSM;

namespace DadVSMe.Players.FSM
{
    public class DashInputDecision : FSMDecision
    {
        public override bool MakeDecision()
        {
            PlayerInputReader inputReader = InputManager.GetInput<PlayerInputReader>();
            if(inputReader == null)
                return false;

            return inputReader.IsDashed;
        }
    }
}
