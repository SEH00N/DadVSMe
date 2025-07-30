using DadVSMe.Players.Animations;
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

            playerAnimator.RemoveAnimationEventListener(EPlayerAnimationEventType.Trigger, HandleAnimationTriggerEvent);
            playerAnimator.AddAnimationEventListener(EPlayerAnimationEventType.Trigger, HandleAnimationTriggerEvent);

            playerAnimator.RemoveAnimationEventListener(EPlayerAnimationEventType.ComboReadingStart, HandleAnimationComboReadingStartEvent);
            playerAnimator.AddAnimationEventListener(EPlayerAnimationEventType.ComboReadingStart, HandleAnimationComboReadingStartEvent);

            playerAnimator.RemoveAnimationEventListener(EPlayerAnimationEventType.ComboReadingEnd, HandleAnimationComboReadingEndEvent);
            playerAnimator.AddAnimationEventListener(EPlayerAnimationEventType.ComboReadingEnd, HandleAnimationComboReadingEndEvent);
        }

        public override void ExitState()
        {
            base.ExitState();

            fsmData.isComboReading = false;
            fsmData.isComboFailed = false;

            playerAnimator.RemoveAnimationEventListener(EPlayerAnimationEventType.Trigger, HandleAnimationTriggerEvent);
            playerAnimator.RemoveAnimationEventListener(EPlayerAnimationEventType.ComboReadingStart, HandleAnimationComboReadingStartEvent);
            playerAnimator.RemoveAnimationEventListener(EPlayerAnimationEventType.ComboReadingEnd, HandleAnimationComboReadingEndEvent);
        }

        private void HandleAnimationTriggerEvent(PlayerAnimationEventData eventData)
        {
            // Hit Enemy
        }

        private void HandleAnimationComboReadingStartEvent(PlayerAnimationEventData eventData)
        {
            fsmData.isComboReading = true;
        }

        private void HandleAnimationComboReadingEndEvent(PlayerAnimationEventData eventData)
        {
            if(fsmData.isComboReading)
                fsmData.isComboFailed = true;

            fsmData.isComboReading = false;
        }
    }
}