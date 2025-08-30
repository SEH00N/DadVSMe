using System.Collections.Generic;
using DadVSMe.Entities;
using H00N.Extensions;
using H00N.Resources.Addressables;
using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe
{
    public struct InitializeAttackFeedback
    {
        public InitializeAttackFeedback(IAttackFeedbackDataContainer feedbackDataContainer)
        {
            foreach (EAttackAttribute attackAttribute in EnumHelper.GetValues<EAttackAttribute>())
            {
                if (feedbackDataContainer.GetFeedbackData(attackAttribute) == null)
                    continue;

                feedbackDataContainer.GetFeedbackData(attackAttribute)?.attackEffects.ForEach(effect => _ = effect.InitializeAsync());
                feedbackDataContainer.GetFeedbackData(attackAttribute)?.hitEffects.ForEach(effect => _ = effect.InitializeAsync());
                feedbackDataContainer.GetFeedbackData(attackAttribute)?.attackSounds.ForEach(sound => _ = sound.InitializeAsync());
                feedbackDataContainer.GetFeedbackData(attackAttribute)?.hitSounds.ForEach(sound => _ = sound.InitializeAsync());
                _ = feedbackDataContainer.GetFeedbackData(attackAttribute)?.hitText?.InitializeAsync();
            }
        }
    }

    public struct PlayAttackSound
    {
        public PlayAttackSound(IAttackFeedbackDataContainer feedbackDataContainer, EAttackAttribute attackAttribute)
        {
            _ = new PlaySound(feedbackDataContainer.GetFeedbackData(EAttackAttribute.Normal)?.attackSounds);
            if(attackAttribute != EAttackAttribute.Normal)
                _ = new PlaySound(feedbackDataContainer.GetFeedbackData(attackAttribute)?.attackSounds);
        }
    }

    public struct PlayAttackFeedback
    {
        public PlayAttackFeedback(IAttackFeedbackDataContainer feedbackDataContainer, EAttackAttribute attackAttribute, Vector3 targetPosition, Vector3 attackOffset, int forwardDirection)
        {
            Vector3 offset = new Vector3(attackOffset.x * forwardDirection, attackOffset.y, 0f);
            
            feedbackDataContainer.GetFeedbackData(EAttackAttribute.Normal)?.attackEffects.ForEach(effect => _ = new PlayEffect(effect, targetPosition + offset, forwardDirection));
            if(attackAttribute != EAttackAttribute.Normal)
                feedbackDataContainer.GetFeedbackData(attackAttribute)?.attackEffects.ForEach(effect => _ = new PlayEffect(effect, targetPosition + offset, forwardDirection));
        }
    }

    public struct PlayHitFeedback
    {
        public PlayHitFeedback(IAttackFeedbackDataContainer feedbackDataContainer, EAttackAttribute attackAttribute, Vector3 targetPosition, Vector3 attackOffset, int forwardDirection)
        {
            Vector3 offset = new Vector3(attackOffset.x * forwardDirection, attackOffset.y, 0f);

            feedbackDataContainer.GetFeedbackData(EAttackAttribute.Normal)?.hitEffects.ForEach(effect => _ = new PlayEffect(effect, targetPosition + offset, forwardDirection));
            _ = new PlaySound(feedbackDataContainer.GetFeedbackData(EAttackAttribute.Normal)?.hitSounds);

            if(attackAttribute != EAttackAttribute.Normal)
            {
                feedbackDataContainer.GetFeedbackData(attackAttribute)?.hitEffects.ForEach(effect => _ = new PlayEffect(effect, targetPosition + offset, forwardDirection));
                _ = new PlaySound(feedbackDataContainer.GetFeedbackData(attackAttribute)?.hitSounds);
            }
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