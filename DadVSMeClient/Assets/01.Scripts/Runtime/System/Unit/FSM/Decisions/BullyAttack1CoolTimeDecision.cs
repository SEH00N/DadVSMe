using DadVSMe.Enemies.FSM;
using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe
{
    public class BullyAttack1CoolTimeDecision : FSMDecision
    {
        BullyEnemyFSMData fsmData;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);

            fsmData = brain.GetAIData<BullyEnemyFSMData>();
        }

        public override bool MakeDecision()
        {
            return fsmData.currnetAttack1Cooltime <= 0;
        }
    }
}