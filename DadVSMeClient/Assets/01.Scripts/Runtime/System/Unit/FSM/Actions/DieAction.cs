using H00N.AI.FSM;

namespace DadVSMe.Entities.FSM
{
    public class DieAction : FSMAction
    {
        private UnitFSMData fsmData = null;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            fsmData = brain.GetAIData<UnitFSMData>();
        }

        public override void EnterState()
        {
            base.EnterState();
            fsmData.isDie = true;
        }
    }
}
