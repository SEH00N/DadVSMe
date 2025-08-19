using H00N.AI.FSM;

namespace DadVSMe.Enemies.FSM.Decision
{
    public class ButtCooltimeDecision : FSMDecision
    {
        private ButtEnemyFSMData buttEnemyFSMData = null;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            buttEnemyFSMData = brain.GetAIData<ButtEnemyFSMData>();
        }

        public override bool MakeDecision()
        {
            return buttEnemyFSMData.buttTimer <= 0f;
        }
    }
}