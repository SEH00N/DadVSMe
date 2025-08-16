using H00N.AI.FSM;

namespace DadVSMe.Entities
{
    public class HitAction : FSMAction
    {
        protected UnitFSMData unitFSMData = null;
        protected IAttackData attackData = null;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            unitFSMData = brain.GetAIData<UnitFSMData>();
        }

        public override void EnterState()
        {
            base.EnterState();
            attackData = unitFSMData.attackData;
            unitFSMData.attackData = null;
        }
    }
}