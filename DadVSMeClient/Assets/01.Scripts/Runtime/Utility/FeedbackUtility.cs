using System.Collections.Generic;
using H00N.Extensions;
using H00N.Resources.Addressables;
using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe
{
    public struct PlayEffect
    {
        public PlayEffect(AddressableAsset<PoolableEffect> effect, Vector3 position, int forwardDirection)
        {
            if(effect == null || string.IsNullOrEmpty(effect.Key))
                return;

            PoolableEffect poolableEffect = PoolManager.Spawn<PoolableEffect>(effect.Key);
            position.z += -0.01f;
            poolableEffect.transform.position = new Vector3(position.x, position.y, position.z);
            poolableEffect.transform.localScale = new Vector3(poolableEffect.transform.localScale.x * Mathf.Sign(forwardDirection), poolableEffect.transform.localScale.y, poolableEffect.transform.localScale.z);
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
            if(sound == null || string.IsNullOrEmpty(sound.Key))
                return;

            AudioManager.Instance.PlaySFX(sound);
        }
    }
}