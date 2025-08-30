using DadVSMe.Enemies.FSM;
using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe.Enemies.FSM
{
    public class SunchipsShootCountDecision : FSMDecision
    {
        [SerializeField] private int targetShootCount;

        private SunchipsEnemyFSMData fsmData;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);

            fsmData = brain.GetAIData<SunchipsEnemyFSMData>();
        }
        
        public override bool MakeDecision()
        {
            return fsmData.shootCount >= targetShootCount;
        }
    }
}
