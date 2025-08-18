using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe.Animals
{
    public abstract class Projectile : PoolReference
    {
        public virtual void Initialize(Vector3 targetPosition) { }
    }
}