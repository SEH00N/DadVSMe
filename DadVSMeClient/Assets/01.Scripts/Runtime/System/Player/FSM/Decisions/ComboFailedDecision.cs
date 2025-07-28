using H00N.AI.FSM;

namespace DadVSMe.Players.FSM
{
    public class ComboFailedDecision : FSMDecision
    {
        private AIData aiData = null;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            aiData = brain.GetAIData<AIData>();
        }

        public override bool MakeDecision()
        {
            return aiData.isComboFailed;
        }
    }
}