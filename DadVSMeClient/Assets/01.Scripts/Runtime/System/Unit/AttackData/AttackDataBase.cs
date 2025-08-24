using System.Collections.Generic;
using H00N.Resources.Addressables;
using UnityEngine;

namespace DadVSMe.Entities
{
    public abstract class AttackDataBase : ScriptableObject, IAttackData
    {
        [SerializeField] int damage = 0;
        public int Damage => damage;

        [Space(10f)]
        [SerializeField] bool isRangeAttack = false;
        public bool IsRangeAttack => isRangeAttack;

        public abstract EAttackFeedback AttackFeedback { get; }

        [SerializeField] List<FeedbackData> feedbackDatas;
        public FeedbackData GetFeedbackData(EAttackAttribute attackAttribute)
            => feedbackDatas.Find(x => x.attackAttribute == attackAttribute);
    }

    [System.Serializable]
    public class FeedbackData
    {
        public EAttackAttribute attackAttribute;

        public List<AddressableAsset<PoolableEffect>> hitEffects = new List<AddressableAsset<PoolableEffect>>();
        public List<AddressableAsset<AudioClip>> attackSounds = new List<AddressableAsset<AudioClip>>();
        public List<AddressableAsset<AudioClip>> hitSounds = new List<AddressableAsset<AudioClip>>();
        public AddressableAsset<DamageText> hitText;
    }
}
