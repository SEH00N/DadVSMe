using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe
{
    public class PoolableAnimationEffect : PoolReference
    {
        [SerializeField] Animator animator = null;

        public void Play() { }
    }
}
