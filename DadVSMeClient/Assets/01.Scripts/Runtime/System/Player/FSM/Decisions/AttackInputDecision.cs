using DadVSMe.Inputs;
using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe.Players.FSM
{
    public class AttackInputDecision : FSMDecision
    {
        [SerializeField] bool attack1 = true;
        [SerializeField] bool attack2 = true;

        public override bool MakeDecision()
        {
            PlayerInputReader inputReader = InputManager.GetInput<PlayerInputReader>();
            if(inputReader == null)
                return false;

            bool attack1Check = attack1 == false || inputReader.GetAttack1Down();
            bool attack2Check = attack2 == false || inputReader.GetAttack2Down();
            return attack1Check && attack2Check;
        }
    }
}
