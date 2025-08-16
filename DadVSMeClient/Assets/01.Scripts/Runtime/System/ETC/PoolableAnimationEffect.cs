using DadVSMe.Entities;
using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe
{
    public class PoolableAnimationEffect : PoolableEffect
    {
        [SerializeField] Animator animator = null;
        [SerializeField] EntityAnimationEventListener animationEventListener = null;

        private bool initialized = false;

        public override void Play() 
        { 
            if(initialized)
                return;

            initialized = true;
            animationEventListener.AddEventListener(EEntityAnimationEventType.End, HandleAnimationEndEvent);
        }

        private void HandleAnimationEndEvent(EntityAnimationEventData eventData) 
        { 
            PoolManager.Despawn(this);
        }
    }
}
