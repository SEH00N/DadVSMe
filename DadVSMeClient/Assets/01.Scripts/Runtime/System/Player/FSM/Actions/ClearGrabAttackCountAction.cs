using H00N.AI.FSM;

namespace DadVSMe.Players.FSM
{
    public class ClearGrabAttackCountAction : FSMAction
    {
        private PlayerFSMData fsmData = null;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            fsmData = brain.GetAIData<PlayerFSMData>();
        }

        public override void EnterState()
        {
            fsmData.grabAttackCount = 0;
        }
    }
}
