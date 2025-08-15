using UnityEngine;
using H00N.AI.FSM;
using System.Collections.Generic;

namespace DadVSMe.Entities
{
    public class AttackFeedbackDecision : FSMDecision
    {
        [SerializeField] List<EAttackFeedback> attackFeedbacks = new List<EAttackFeedback>();
        private HashSet<EAttackFeedback> attackFeedbackSet = new HashSet<EAttackFeedback>();

        private UnitFSMData unitFSMData = null;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            unitFSMData = brain.GetAIData<UnitFSMData>();
            attackFeedbackSet = new HashSet<EAttackFeedback>(attackFeedbacks);
        }

        public override bool MakeDecision()
        {
            if(unitFSMData.attackData == null)
                return false;

            return attackFeedbackSet.Contains(unitFSMData.attackData.AttackFeedback);
        }
    }
}
