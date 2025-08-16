using System.Collections.Generic;
using H00N.Extensions;
using H00N.Resources.Addressables;
using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe
{
    public struct PlayEffect
    {
        public PlayEffect(AddressableAsset<ParticleSystem> effect, Vector3 position)
        {
            if(effect == null || string.IsNullOrEmpty(effect.Key) || effect.Initialized == false)
                return;

            ParticleSystem particle = PoolManager.Spawn<ParticleSystem>(effect);
            particle.transform.position = position;
            particle.Play();
        }
        
        public PlayEffect(AddressableAsset<Animator> effect, Vector3 position)
        {
            if(effect == null)
                return;

            Animator animator = PoolManager.Spawn<Animator>(effect);
            animator.transform.position = position;
        }
    }

    public struct PlaySound
    {
        public PlaySound(List<AddressableAsset<AudioClip>> sounds)
        {
            if(sounds == null)
                return;

            AddressableAsset<AudioClip> sound = sounds.PickRandom();
            if(sound == null || string.IsNullOrEmpty(sound.Key) || sound.Initialized == false)
                return;

            AudioManager.Instance.PlaySFX(sound);
        }
    }
}