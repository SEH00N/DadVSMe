using H00N.AI.FSM;
using DadVSMe.Entities;

namespace DadVSMe.Players.FSM
{
    public class PlayerAttackAction : FSMAction
    {
        private PlayerFSMData fsmData = null;
        private EntityAnimator entityAnimator = null;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            fsmData = brain.GetAIData<PlayerFSMData>();
            entityAnimator = brain.GetComponent<EntityAnimator>();
        }

        public override void EnterState()
        {
            base.EnterState();

            fsmData.isComboReading = false;
            fsmData.isComboFailed = false;

            entityAnimator.RemoveAnimationEventListener(EEntityAnimationEventType.ComboReadingStart, HandleAnimationComboReadingStartEvent);
            entityAnimator.AddAnimationEventListener(EEntityAnimationEventType.ComboReadingStart, HandleAnimationComboReadingStartEvent);

            entityAnimator.RemoveAnimationEventListener(EEntityAnimationEventType.ComboReadingEnd, HandleAnimationComboReadingEndEvent);
            entityAnimator.AddAnimationEventListener(EEntityAnimationEventType.ComboReadingEnd, HandleAnimationComboReadingEndEvent);
        }

        public override void ExitState()
        {
            base.ExitState();

            fsmData.isComboReading = false;
            fsmData.isComboFailed = false;

            entityAnimator.RemoveAnimationEventListener(EEntityAnimationEventType.ComboReadingStart, HandleAnimationComboReadingStartEvent);
            entityAnimator.RemoveAnimationEventListener(EEntityAnimationEventType.ComboReadingEnd, HandleAnimationComboReadingEndEvent);
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
