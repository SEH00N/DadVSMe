using DadVSMe.Enemies.FSM;
using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe
{
    public class ShootCooltimeDecision : FSMDecision
    {
        SunchipsEnemyFSMData data;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);

            data = brain.GetAIData<SunchipsEnemyFSMData>();
        }

        public override bool MakeDecision()
        {
            return data.shootCooltime <= 0f;
        }
    }
}
