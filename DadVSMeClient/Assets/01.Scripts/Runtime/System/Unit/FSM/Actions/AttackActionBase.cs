using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe.Entities.FSM
{
    public abstract class AttackActionBase : FSMAction
    {
        [SerializeField] protected Vector2 attackOffset = Vector2.zero;
        protected abstract IAttackFeedbackDataContainer FeedbackDataContainer { get; }

        protected EntityAnimator entityAnimator = null;
        protected UnitFSMData unitFSMData = null;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            entityAnimator = brain.GetComponent<EntityAnimator>();
            unitFSMData = brain.GetAIData<UnitFSMData>();

            _ = new InitializeAttackFeedback(FeedbackDataContainer);
        }

        public override void EnterState()
        {
            base.EnterState();

            entityAnimator.RemoveAnimationEventListener(EEntityAnimationEventType.Trigger, HandleAnimationTriggerEvent);
            entityAnimator.AddAnimationEventListener(EEntityAnimationEventType.Trigger, HandleAnimationTriggerEvent);

            _ = new PlayAttackSound(FeedbackDataContainer, unitFSMData.attackAttribute);
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

        protected virtual void AttackToTarget(Unit target, IAttackData attackData, bool playEffect = true)
        {
            target.UnitHealth.Attack(unitFSMData.unit, attackData);
            unitFSMData.unit.onAttackTargetEvent?.Invoke(target, attackData);

            if (playEffect)
                _ = new PlayHitFeedback(FeedbackDataContainer, unitFSMData.attackAttribute, target.transform.position, attackOffset, unitFSMData.forwardDirection);
        }

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if(UnityEditor.Selection.activeObject != gameObject)
                return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + (Vector3)attackOffset, 0.25f);
        }
        #endif
    }
}