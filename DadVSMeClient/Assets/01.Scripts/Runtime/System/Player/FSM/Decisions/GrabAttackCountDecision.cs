using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe.Players.FSM
{
    public class GrabAttackCountDecision : FSMDecision
    {
        private enum ECompareType
        {
            Equal,
            GreaterThan,
            GreaterEqualThan,
            LessThan,
            LessEqualThan,
        }

        [SerializeField] ECompareType compareType = ECompareType.Equal;
        [SerializeField] int compareCount = 0;

        private PlayerFSMData fsmData = null;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            fsmData = brain.GetAIData<PlayerFSMData>();
        }

        public override bool MakeDecision()
        {
            return compareType switch {
                ECompareType.Equal => fsmData.grabAttackCount == compareCount,
                ECompareType.GreaterThan => fsmData.grabAttackCount > compareCount,
                ECompareType.GreaterEqualThan => fsmData.grabAttackCount >= compareCount,
                ECompareType.LessThan => fsmData.grabAttackCount < compareCount,
                ECompareType.LessEqualThan => fsmData.grabAttackCount <= compareCount,
                _ => false,
            };
        }
    }
}
