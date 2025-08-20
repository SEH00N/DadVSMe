using Cysharp.Threading.Tasks;
using DadVSMe.Entities;
using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe.Animals
{
    public class Animal : Entity
    {
        [Header("Animal")]
        [SerializeField] Transform firePosition = null;
        [SerializeField] AnimalMovement animalMovement = null;

        private Unit owner = null;
        private AnimalEntityData animalEntityData = null;
        private Vector3 targetPositionCache = Vector3.zero;

        protected override void InitializeInternal(IEntityData data)
        {
            base.InitializeInternal(data);
            animalEntityData = data as AnimalEntityData;
            animalEntityData.ProjectilePrefab.InitializeAsync().Forget();

            entityAnimator.AddAnimationEventListener(EEntityAnimationEventType.Trigger, HandleAnimationTriggerEvent);
            entityAnimator.AddAnimationEventListener(EEntityAnimationEventType.End, HandleAnimationEndEvent);
        }

        public void SetOwner(Unit owner)
        {
            this.owner = owner;
        }

        public void SetFollowTarget(Transform followTarget)
        {
            animalMovement.SetFollowTarget(followTarget);
        }

        public void Fire(Vector3 targetPosition)
        {
            targetPositionCache = targetPosition;
            entityAnimator.PlayAnimation("Attack");
        }

        private void HandleAnimationTriggerEvent(EntityAnimationEventData eventData)
        {
            Projectile projectile = PoolManager.Spawn<Projectile>(animalEntityData.ProjectilePrefab.Key);
            projectile.transform.position = firePosition.position;
            projectile.Initialize(owner, targetPositionCache);
        }

        private void HandleAnimationEndEvent(EntityAnimationEventData eventData)
        {
            entityAnimator.PlayDefaultAnimation();
        }
    }
}
