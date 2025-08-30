using DadVSMe.Entities;
using H00N.Resources.Pools;
using UnityEngine;
using UnityEngine.Events;

namespace DadVSMe
{
    public abstract class Projectile : PoolReference
    {
        [SerializeField] UnityEvent onInitializedEvent = null;

        protected Unit owner = null;

        public virtual void Initialize(Unit owner, Vector2 targetPosition)
        {
            this.owner = owner;
            onInitializedEvent?.Invoke();
        }
    }
}