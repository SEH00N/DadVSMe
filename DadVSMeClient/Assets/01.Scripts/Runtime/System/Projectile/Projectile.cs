using DadVSMe.Entities;
using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe
{
    public abstract class Projectile : PoolReference
    {
        protected Unit owner = null;

        public virtual void Initialize(Unit owner, Vector2 targetPosition)
        {
            this.owner = owner;
        }
    }
}