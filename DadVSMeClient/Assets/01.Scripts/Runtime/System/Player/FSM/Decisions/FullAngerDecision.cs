using H00N.AI.FSM;

namespace DadVSMe.Players.FSM
{
    public class FullAngerDecision : FSMDecision
    {
        private PlayerFSMData fsmData;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);

            fsmData = brain.GetAIData<PlayerFSMData>();
        }

        public override bool MakeDecision()
        {
            if(fsmData == null)
                return false;
            
            if(fsmData.isAnger)
                return false;

            return fsmData.currentAngerGauge >= fsmData.maxAngerGauge;
        }
    }
}
