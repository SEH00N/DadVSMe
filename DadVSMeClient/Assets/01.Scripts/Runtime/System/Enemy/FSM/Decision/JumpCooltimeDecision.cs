using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe.Enemies.FSM.Decision
{
    public class JumpCooltimeDecision : FSMDecision
    {
        private NinjaFSMData _ninjaFSMData = null;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            _ninjaFSMData = brain.GetAIData<NinjaFSMData>();
        }

        public override bool MakeDecision()
        {
            return _ninjaFSMData.jumpAttackTimer <= 0;
        }
    }
}
