using System.Collections.Generic;
using H00N.Resources.Addressables;
using UnityEngine;

namespace DadVSMe.Entities
{
    public abstract class AttackDataBase : ScriptableObject, IAttackData, IAttackFeedbackDataContainer
    {
        [SerializeField] int damage = 0;
        public int Damage => damage;

        [Space(10f)]
        [SerializeField] bool isRageAttack = false;
        public bool IsRageAttack => isRageAttack;

        public abstract EAttackFeedback AttackFeedback { get; }

        [SerializeField] List<FeedbackData> feedbackDatas;
        public FeedbackData GetFeedbackData(EAttackAttribute attackAttribute) => feedbackDatas.Find(x => x.attackAttribute == attackAttribute);
    }
}
