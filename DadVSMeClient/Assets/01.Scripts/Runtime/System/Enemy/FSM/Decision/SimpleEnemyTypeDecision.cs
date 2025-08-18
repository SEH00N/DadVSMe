using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe.Enemies.FSM
{
    public class SimpleEnemyTypeDecision : FSMDecision
    {
        [SerializeField] ESimpleEnemyType compareType;
        private SimpleEnemyFSMData simpleEnemyFSMData = null;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            simpleEnemyFSMData = brain.GetAIData<SimpleEnemyFSMData>();
        }

        public override bool MakeDecision()
        {
            return simpleEnemyFSMData.enemyType == compareType;
        }
    }
}
