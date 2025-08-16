using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe
{
    public class PoolableParticleEffect : PoolReference
    {
        [SerializeField] ParticleSystem particle = null;

        public void Play()
        {
            particle.Play();
        }
    }
}
