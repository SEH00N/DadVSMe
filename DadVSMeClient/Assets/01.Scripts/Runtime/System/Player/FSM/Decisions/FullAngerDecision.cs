using DadVSMe.Entities;
using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe.Players.FSM
{
    public class FullAngerDecision : FSMDecision
    {
        private PlayerFSMData fsmData;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);

            fsmData = brain.GetAIData<PlayerFSMData>();
        }

        public override bool MakeDecision()
        {
            return fsmData.currentAngerGauge >= fsmData.maxAngerGauge;
        }
    }
}
