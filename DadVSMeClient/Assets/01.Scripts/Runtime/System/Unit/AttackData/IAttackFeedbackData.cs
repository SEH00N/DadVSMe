using System.Collections.Generic;
using H00N.Resources.Addressables;
using UnityEngine;

namespace DadVSMe.Entities
{
    public interface IAttackFeedbackDataContainer
    {
        public FeedbackData GetFeedbackData(EAttackAttribute attackAttribute);
    }

    [System.Serializable]
    public class FeedbackData
    {
        public EAttackAttribute attackAttribute;

        public List<AddressableAsset<PoolableEffect>> attackEffects = new List<AddressableAsset<PoolableEffect>>();
        public List<AddressableAsset<PoolableEffect>> hitEffects = new List<AddressableAsset<PoolableEffect>>();
        public List<AddressableAsset<AudioClip>> attackSounds = new List<AddressableAsset<AudioClip>>();
        public List<AddressableAsset<AudioClip>> hitSounds = new List<AddressableAsset<AudioClip>>();
        public AddressableAsset<DamageText> hitText;
    }
}