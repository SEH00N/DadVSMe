using DadVSMe.Entities;
using H00N.AI.FSM;

namespace DadVSMe.Players.FSM
{
    public class AttackAction : FSMAction
    {
        private PlayerAnimator playerAnimator = null;
        private PlayerFSMData fsmData = null;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            fsmData = brain.GetAIData<PlayerFSMData>();
            playerAnimator = brain.GetComponent<PlayerAnimator>();
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