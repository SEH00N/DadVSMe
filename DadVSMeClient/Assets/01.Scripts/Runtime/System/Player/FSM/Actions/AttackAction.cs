using DadVSMe.Players.Animations;
using H00N.AI.FSM;

namespace DadVSMe.Players.FSM
{
    public class AttackAction : FSMAction
    {
        private PlayerAnimator playerAnimator = null;
        private AIData aiData = null;

        private void Awake()
        {
            playerAnimator = brain.GetComponent<PlayerAnimator>();
        }

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            aiData = brain.GetAIData<AIData>();
        }

        public override void EnterState()
        {
            base.EnterState();

            aiData.isComboReading = false;
            aiData.isComboFailed = false;

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

            aiData.isComboReading = false;
            aiData.isComboFailed = false;

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
            aiData.isComboReading = true;
        }

        private void HandleAnimationComboReadingEndEvent(PlayerAnimationEventData eventData)
        {
            if(aiData.isComboReading)
                aiData.isComboFailed = true;

            aiData.isComboReading = false;
        }
    }
}