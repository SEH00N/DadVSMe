using System.Collections.Generic;
using H00N.Extensions;
using H00N.Resources.Addressables;
using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe
{
    public struct PlayEffect
    {
        public PlayEffect(AddressableAsset<PoolableEffect> effect, Vector3 position)
        {
            if(effect == null || string.IsNullOrEmpty(effect.Key) || effect.Initialized == false)
                return;

            PoolableEffect poolableEffect = PoolManager.Spawn<PoolableEffect>(effect.Key);
            poolableEffect.transform.position = position;
            poolableEffect.Play();
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