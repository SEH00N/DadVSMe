using DadVSMe.Entities;
using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe.Players.FSM
{
    public class AttackAction : FSMAction
    {
        [SerializeField] int damage = 5;
        [SerializeField] EAttackFeedback attackFeedback = EAttackFeedback.NormalHit1;
        [SerializeField] float attackFeedbackValue = 1f;

        private EntityAnimator playerAnimator = null;
        private PlayerFSMData fsmData = null;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            fsmData = brain.GetAIData<PlayerFSMData>();
            playerAnimator = brain.GetComponent<EntityAnimator>();
        }

        public override void EnterState()
        {
            base.EnterState();

            fsmData.isComboReading = false;
            fsmData.isComboFailed = false;

            playerAnimator.RemoveAnimationEventListener(EEntityAnimationEventType.Trigger, HandleAnimationTriggerEvent);
            playerAnimator.AddAnimationEventListener(EEntityAnimationEventType.Trigger, HandleAnimationTriggerEvent);

            playerAnimator.RemoveAnimationEventListener(EEntityAnimationEventType.ComboReadingStart, HandleAnimationComboReadingStartEvent);
            playerAnimator.AddAnimationEventListener(EEntityAnimationEventType.ComboReadingStart, HandleAnimationComboReadingStartEvent);

            playerAnimator.RemoveAnimationEventListener(EEntityAnimationEventType.ComboReadingEnd, HandleAnimationComboReadingEndEvent);
            playerAnimator.AddAnimationEventListener(EEntityAnimationEventType.ComboReadingEnd, HandleAnimationComboReadingEndEvent);
        }

        public override void ExitState()
        {
            base.ExitState();

            fsmData.isComboReading = false;
            fsmData.isComboFailed = false;

            playerAnimator.RemoveAnimationEventListener(EEntityAnimationEventType.Trigger, HandleAnimationTriggerEvent);
            playerAnimator.RemoveAnimationEventListener(EEntityAnimationEventType.ComboReadingStart, HandleAnimationComboReadingStartEvent);
            playerAnimator.RemoveAnimationEventListener(EEntityAnimationEventType.ComboReadingEnd, HandleAnimationComboReadingEndEvent);
        }

        private void HandleAnimationTriggerEvent(EntityAnimationEventData eventData)
        {
            // Hit Enemy


            // test
            Unit enemyUnit = fsmData.grabbedEntity as Unit;
            enemyUnit.UnitHealth.Attack(damage, attackFeedback, attackFeedbackValue);
        }

        private void HandleAnimationComboReadingStartEvent(EntityAnimationEventData eventData)
        {
            fsmData.isComboReading = true;
        }

        private void HandleAnimationComboReadingEndEvent(EntityAnimationEventData eventData)
        {
            if(fsmData.isComboReading)
                fsmData.isComboFailed = true;

            fsmData.isComboReading = false;
        }
    }
}