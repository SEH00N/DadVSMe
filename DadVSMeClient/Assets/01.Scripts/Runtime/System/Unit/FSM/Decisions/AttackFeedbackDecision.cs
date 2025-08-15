using UnityEngine;
using H00N.AI.FSM;
using ShibaInspector.Attributes;

namespace DadVSMe.Entities
{
    public class AttackFeedbackDecision : FSMDecision
    {
        [SerializeField] private bool any = true;
        [ConditionalField(nameof(any), false, true), SerializeField] private EAttackFeedback attackFeedback = EAttackFeedback.None;

        private UnitFSMData unitFSMData = null;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            unitFSMData = brain.GetAIData<UnitFSMData>();
        }

        public override bool MakeDecision()
        {
            if(unitFSMData.attackData == null)
                return false;

            if(any)
                return true;

            return unitFSMData.attackData.AttackFeedback == attackFeedback;
        }
    }
}
