using H00N.AI.FSM;

namespace DadVSMe.Players.FSM
{
    public class ReleaseComboAction : FSMAction
    {
        private AIData aiData = null;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            aiData = brain.GetAIData<AIData>();
        }

        public override void EnterState()
        {
            base.EnterState();

            aiData.isComboReading = false;
            aiData.isComboFailed = false;
        }
    }
}