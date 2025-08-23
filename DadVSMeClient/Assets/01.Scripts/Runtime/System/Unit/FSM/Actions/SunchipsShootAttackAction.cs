using DadVSMe.Enemies.FSM;
using DadVSMe.Entities;
using DadVSMe.Entities.FSM;
using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe
{
    public class SunchipsShootAttackAction : ShootAttackAction
    {
        SunchipsEnemyFSMData data;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);

            data = brain.GetAIData<SunchipsEnemyFSMData>();
        }

        public override void EnterState()
        {
            base.EnterState();

            data.shootCount++;
        }
    }
}
