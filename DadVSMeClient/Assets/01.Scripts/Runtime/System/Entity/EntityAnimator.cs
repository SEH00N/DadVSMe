using UnityEngine;
using System;
using System.Collections.Generic;

namespace DadVSMe.Entities
{
    public class EntityAnimator : MonoBehaviour
    {
        [SerializeField] string defaultAnimationName = "Idle";

        [Space(10f)]
        [SerializeField] Animator animator = null;
        [SerializeField] EntityAnimationEventListener animationEventListener = null;

        private Dictionary<string, int> animationHashTable = new Dictionary<string, int>();

        public void Initialize()
        {
            animationEventListener.Clear();
            PlayDefaultAnimation();
        }

        public void PlayDefaultAnimation() => PlayAnimation(defaultAnimationName);
        public void PlayAnimation(string animationName)
        {
            if(animationHashTable.TryGetValue(animationName, out int hash) == false)
            {
                hash = Animator.StringToHash(animationName);
                animationHashTable.Add(animationName, hash);
            }

            animator.Play(hash, 0, 0f);
            animator.Update(0f);
        }

        public void SetRotation(bool isRight)
        {
            float targetScaleX = isRight ? 1 : -1;
            if(transform.localScale.x == targetScaleX)
                return;

            transform.localScale = new Vector3(targetScaleX, 1, 1);
        }

        public void AddAnimationEventListener(EEntityAnimationEventType eventType, Action<EntityAnimationEventData> action) => animationEventListener.AddEventListener(eventType, action);
        public void RemoveAnimationEventListener(EEntityAnimationEventType eventType, Action<EntityAnimationEventData> action) => animationEventListener.RemoveEventListener(eventType, action);
    }
}