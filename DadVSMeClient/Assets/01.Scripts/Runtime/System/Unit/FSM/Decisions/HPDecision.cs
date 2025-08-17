using H00N.AI.FSM;

namespace DadVSMe.Entities.FSM
{
    public class HPDecision : FSMDecision
    {
        private UnitHealth unitHealth = null;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            unitHealth = brain.GetComponent<UnitHealth>();
        }

        public override bool MakeDecision()
        {
            return unitHealth.CurrentHP <= 0;
        }
    }
}
