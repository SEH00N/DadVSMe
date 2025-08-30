using DadVSMe.Enemies.FSM;
using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe.Enemies.FSM
{
    public class SunchipsSpinCooltimeDecision : FSMDecision
    {
        SunchipsEnemyFSMData fsmData;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);

            fsmData = brain.GetAIData<SunchipsEnemyFSMData>();
        }

        public override bool MakeDecision()
        {
            return fsmData.currentFrenzyCooltime <= 0f;
        }
    }
}
