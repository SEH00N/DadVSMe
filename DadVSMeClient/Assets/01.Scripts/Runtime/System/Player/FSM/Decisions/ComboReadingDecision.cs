using H00N.AI.FSM;

namespace DadVSMe.Players.FSM
{
    public class ComboReadingDecision : FSMDecision
    {
        private PlayerFSMData aiData = null;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            aiData = brain.GetAIData<PlayerFSMData>();
        }

        public override bool MakeDecision()
        {
            return aiData.isComboReading;
        }
    }
}