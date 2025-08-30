using DadVSMe.Enemies.FSM;
using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe.Enemies.FSM
{
    public class BullyAttack2CoolTimeDecision : FSMDecision
    {
        BullyEnemyFSMData fsmData;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);

            fsmData = brain.GetAIData<BullyEnemyFSMData>();
        }

        public override bool MakeDecision()
        {
            return fsmData.currnetAttack2Cooltime <= 0;
        }
    }
}