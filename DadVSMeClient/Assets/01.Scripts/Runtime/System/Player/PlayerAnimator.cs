using DadVSMe.Players.Animations;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace DadVSMe.Players
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] Animator animator = null;
        [SerializeField] PlayerAnimationEventListener animationEventListener = null;

        private const string DEFAULT_ANIMATION_NAME = "idle";
        private Dictionary<string, int> animationHashTable = new Dictionary<string, int>();

        public void Initialize()
        {
            animationEventListener.Clear();
            PlayDefaultAnimation();
        }

        public void PlayDefaultAnimation() => PlayAnimation(DEFAULT_ANIMATION_NAME);
        public void PlayAnimation(string animationName)
        {
            if(animationHashTable.TryGetValue(animationName, out int hash) == false)
            {
                hash = Animator.StringToHash(animationName);
                animationHashTable.Add(animationName, hash);
            }

            animator.Play(hash);
        }

        public void AddAnimationEventListener(EPlayerAnimationEventType eventType, Action<PlayerAnimationEventData> action) => animationEventListener.AddEventListener(eventType, action);
        public void RemoveAnimationEventListener(EPlayerAnimationEventType eventType, Action<PlayerAnimationEventData> action) => animationEventListener.RemoveEventListener(eventType, action);
    }
}