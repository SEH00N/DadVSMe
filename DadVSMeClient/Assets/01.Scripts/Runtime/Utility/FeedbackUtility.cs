using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DadVSMe.Core.Cam;
using DadVSMe.Entities;
using H00N.Extensions;
using H00N.Resources.Addressables;
using H00N.Resources.Pools;
using UnityEngine;
using UnityEngine.UI;

namespace DadVSMe
{
    public struct InitializeAttackFeedback
    {
        public InitializeAttackFeedback(AttackDataBase attackData)
        {
            foreach (EAttackAttribute attackAttribute in EnumHelper.GetValues<EAttackAttribute>())
            {
                if (attackData.GetFeedbackData(attackAttribute) == null)
                    continue;

                attackData.GetFeedbackData(attackAttribute)?.hitEffects.ForEach(effect => effect.InitializeAsync().Forget());
                attackData.GetFeedbackData(attackAttribute)?.attackSounds.ForEach(sound => sound.InitializeAsync().Forget());
                attackData.GetFeedbackData(attackAttribute)?.hitSounds.ForEach(sound => sound.InitializeAsync().Forget());
                attackData.GetFeedbackData(attackAttribute)?.hitText.InitializeAsync().Forget();
            }
        }
    }

    public struct PlayAttackSound
    {
        public PlayAttackSound(AttackDataBase attackData, EAttackAttribute attackAttribute)
        {
            _ = new PlaySound(attackData.GetFeedbackData(EAttackAttribute.Normal)?.attackSounds);
            _ = new PlaySound(attackData.GetFeedbackData(attackAttribute)?.attackSounds);
        }
    }

    public struct PlayAttackFeedback
    {
        public PlayAttackFeedback(AttackDataBase attackData, EAttackAttribute attackAttribute, Vector3 targetPosition, Vector3 attackOffset, int forwardDirection)
        {
            Vector3 offset = new Vector3(attackOffset.x * forwardDirection, attackOffset.y, 0f);

            attackData.GetFeedbackData(EAttackAttribute.Normal)?.hitEffects.ForEach(effect => _ = new PlayEffect(effect, targetPosition + offset, forwardDirection));
            _ = new PlaySound(attackData.GetFeedbackData(EAttackAttribute.Normal)?.hitSounds);

            attackData.GetFeedbackData(attackAttribute)?.hitEffects.ForEach(effect => _ = new PlayEffect(effect, targetPosition + offset, forwardDirection));
            _ = new PlaySound(attackData.GetFeedbackData(attackAttribute)?.hitSounds);
        }
    }

    public struct PlayEffect
    {
        public PlayEffect(AddressableAsset<PoolableEffect> effect, Vector3 position, int forwardDirection)
        {
            if (effect == null || string.IsNullOrEmpty(effect.Key))
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
            if (sounds == null || sounds.Count <= 0)
                return;

            AddressableAsset<AudioClip> sound = sounds.PickRandom();
            PlayInternalAsync(sound);
        }
        
        public PlaySound(AddressableAsset<AudioClip> sound)
        {
            PlayInternalAsync(sound);
        }

        private async void PlayInternalAsync(AddressableAsset<AudioClip> sound)
        {
            if(sound == null || string.IsNullOrEmpty(sound.Key))
                return;

            // not loaded yet
            if(sound.Asset == null)
                await sound.InitializeAsync();

            AudioManager.Instance.PlaySFX(sound);
        }
    }
}