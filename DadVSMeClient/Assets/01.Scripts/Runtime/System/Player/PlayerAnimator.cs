using DadVSMe.Players.Animations;
using UnityEngine;
using System;
using H00N.Resources.Pools;

namespace DadVSMe.Players
{
    public class PlayerAnimator : MonoBehaviour, IPoolableBehaviour
    {
        [SerializeField] Animator animator = null;
        [SerializeField] PoolReference poolReference = null;
        [SerializeField] PlayerAnimationEventListener animationEventListener = null;

        public PoolReference PoolReference => poolReference;

        private static readonly int IS_IDLE_HASH = Animator.StringToHash("is_idle");
        private static readonly int IS_MOVE_HASH = Animator.StringToHash("is_move");

        private int currentHash = 0;
        private bool isNoneHash = false;

        private void Awake()
        {
            currentHash = IS_IDLE_HASH;
        }

        private void OnEnable()
        {
            isNoneHash = true;
        }

        public void OnSpawned() { }
        public void OnDespawn() 
        {
            animationEventListener.Clear();
        }

        public void AddAnimationEventListener(EPlayerAnimationEventType eventType, Action<PlayerAnimationEventData> action) => animationEventListener.AddEventListener(eventType, action);
        public void RemoveAnimationEventListener(EPlayerAnimationEventType eventType, Action<PlayerAnimationEventData> action) => animationEventListener.RemoveEventListener(eventType, action);

        public void SetIdle() => ChangeState(IS_IDLE_HASH);
        public void SetMove() => ChangeState(IS_MOVE_HASH);

        private void ChangeState(int hash)
        {
            if(isNoneHash == false && currentHash == hash)
                return;

            isNoneHash = false;
            animator.SetBool(currentHash, false);
            currentHash = hash;
            animator.SetBool(currentHash, true);
        }
    }
}