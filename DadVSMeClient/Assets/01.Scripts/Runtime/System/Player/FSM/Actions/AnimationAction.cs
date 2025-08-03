using System.Collections.Generic;
using DadVSMe.Players.Animations;
using H00N.AI.FSM;
using UnityEngine;
using UnityEngine.Events;

namespace DadVSMe.Players.FSM
{
    public class AnimationAction : FSMAction
    {
        [System.Serializable]
        private struct AnimationEventData
        {
            public EPlayerAnimationEventType eventType;
            public UnityEvent @event;
        }

        [SerializeField] AnimationEventData[] animationEventDatas = null;
        [SerializeField] Dictionary<EPlayerAnimationEventType, UnityEvent> animationEventDictionary = new Dictionary<EPlayerAnimationEventType, UnityEvent>();

        private PlayerAnimator playerAnimator = null;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            playerAnimator = brain.GetComponent<PlayerAnimator>();

            foreach(AnimationEventData animationEventData in animationEventDatas)
                animationEventDictionary[animationEventData.eventType] = animationEventData.@event;
        }

        public override void EnterState()
        {
            base.EnterState();

            foreach(EPlayerAnimationEventType eventType in animationEventDictionary.Keys)
                playerAnimator.AddAnimationEventListener(eventType, HandleAnimationEvent);
        }

        public override void ExitState()
        {
            base.ExitState();

            foreach(EPlayerAnimationEventType eventType in animationEventDictionary.Keys)
                playerAnimator.RemoveAnimationEventListener(eventType, HandleAnimationEvent);
        }

        private void HandleAnimationEvent(PlayerAnimationEventData eventData)
        {
            if(animationEventDictionary.TryGetValue(eventData.eventType, out UnityEvent @event) == false)
                return;

            @event.Invoke();
        }
    }
}