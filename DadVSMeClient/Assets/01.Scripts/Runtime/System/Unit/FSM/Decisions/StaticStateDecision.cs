using H00N.AI.FSM;

namespace DadVSMe.Entities.FSM
{
    public class StaticStateDecision : FSMDecision
    {
        private UnitFSMData unitFSMData = null;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            unitFSMData = brain.GetAIData<UnitFSMData>();
        }

        public override bool MakeDecision()
        {
            return unitFSMData.unit.StaticEntity;
        }
    }
}
