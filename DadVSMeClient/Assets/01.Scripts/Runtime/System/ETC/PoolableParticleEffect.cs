using UnityEngine;

namespace DadVSMe
{
    public class PoolableParticleEffect : PoolableEffect
    {
        [SerializeField] ParticleSystem particle = null;

        public override void Play()
        {
            particle.Play();
        }
    }
}
