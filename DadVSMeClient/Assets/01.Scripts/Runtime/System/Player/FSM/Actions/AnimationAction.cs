using DadVSMe.Players.Animations;
using H00N.AI.FSM;
using UnityEngine;
using UnityEngine.Events;

namespace DadVSMe.Players.FSM
{
    public class AnimationAction : FSMAction
    {
        [SerializeField] UnityEvent onAnimationStartEvent = null;
        [SerializeField] UnityEvent onAnimationEndEvent = null;

        private PlayerAnimator playerAnimator = null;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            playerAnimator = brain.GetComponent<PlayerAnimator>();
        }

        public override void EnterState()
        {
            base.EnterState();

            playerAnimator.RemoveAnimationEventListener(EPlayerAnimationEventType.Start, HandleAnimationStartEvent);
            playerAnimator.AddAnimationEventListener(EPlayerAnimationEventType.Start, HandleAnimationStartEvent);

            playerAnimator.RemoveAnimationEventListener(EPlayerAnimationEventType.End, HandleAnimationEndEvent);
            playerAnimator.AddAnimationEventListener(EPlayerAnimationEventType.End, HandleAnimationEndEvent);
        }

        public override void ExitState()
        {
            base.ExitState();

            playerAnimator.RemoveAnimationEventListener(EPlayerAnimationEventType.Start, HandleAnimationStartEvent);
            playerAnimator.RemoveAnimationEventListener(EPlayerAnimationEventType.End, HandleAnimationEndEvent);
        }

        private void HandleAnimationStartEvent(PlayerAnimationEventData eventData)
        {
            onAnimationStartEvent?.Invoke();
        }

        private void HandleAnimationEndEvent(PlayerAnimationEventData eventData)
        {
            onAnimationEndEvent?.Invoke();
        }
    }
}