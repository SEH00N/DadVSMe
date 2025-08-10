using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe.Entities.FSM
{
    public abstract class AttackActionBase : FSMAction
    {
        [SerializeField] private AttackData attackData = null;

        private EntityAnimator entityAnimator = null;
        private UnitFSMData unitFSMData = null;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            entityAnimator = brain.GetComponent<EntityAnimator>();
            unitFSMData = brain.GetAIData<UnitFSMData>();
        }

        public override void EnterState()
        {
            base.EnterState();

            entityAnimator.RemoveAnimationEventListener(EEntityAnimationEventType.Trigger, HandleAnimationTriggerEvent);
            entityAnimator.AddAnimationEventListener(EEntityAnimationEventType.Trigger, HandleAnimationTriggerEvent);
        }

        public override void ExitState()
        {
            base.ExitState();

            entityAnimator.RemoveAnimationEventListener(EEntityAnimationEventType.Trigger, HandleAnimationTriggerEvent);
        }

        protected abstract void OnAttack(EntityAnimationEventData eventData);
        private void HandleAnimationTriggerEvent(EntityAnimationEventData eventData)
        {
            OnAttack(eventData);
        }

        protected void AttackToTarget(Unit target)
        {
            target.UnitHealth.Attack(unitFSMData.unit, attackData);
        }
    }
}