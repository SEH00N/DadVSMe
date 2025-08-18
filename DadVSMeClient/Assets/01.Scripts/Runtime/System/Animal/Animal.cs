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
        private AnimalEntityData animalEntityData = null;

        protected override void InitializeInternal(IEntityData data)
        {
            base.InitializeInternal(data);
            animalEntityData = data as AnimalEntityData;
            animalEntityData.ProjectilePrefab.InitializeAsync().Forget();
        }

        public void SetFollowTarget(Transform followTarget)
        {
            animalMovement.SetFollowTarget(followTarget);
        }

        public void Fire(Vector3 targetPosition)
        {
            Projectile projectile = PoolManager.Spawn<Projectile>(animalEntityData.ProjectilePrefab.Key);
            projectile.transform.position = firePosition.position;
            projectile.Initialize(targetPosition);
        }
    }
}
