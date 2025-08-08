using System.Collections.Generic;
using DadVSMe.Entities;
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
            public EEntityAnimationEventType eventType;
            public UnityEvent @event;
        }

        [SerializeField] AnimationEventData[] animationEventDatas = null;
        [SerializeField] Dictionary<EEntityAnimationEventType, UnityEvent> animationEventDictionary = new Dictionary<EEntityAnimationEventType, UnityEvent>();

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

            foreach(EEntityAnimationEventType eventType in animationEventDictionary.Keys)
                playerAnimator.AddAnimationEventListener(eventType, HandleAnimationEvent);
        }

        public override void ExitState()
        {
            base.ExitState();

            foreach(EEntityAnimationEventType eventType in animationEventDictionary.Keys)
                playerAnimator.RemoveAnimationEventListener(eventType, HandleAnimationEvent);
        }

        private void HandleAnimationEvent(EntityAnimationEventData eventData)
        {
            if(animationEventDictionary.TryGetValue(eventData.eventType, out UnityEvent @event) == false)
                return;

            @event.Invoke();
        }
    }
}