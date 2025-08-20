using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe
{
    public abstract class Projectile : PoolReference
    {
        public virtual void Initialize(Vector2 targetPosition) { }
    }
}