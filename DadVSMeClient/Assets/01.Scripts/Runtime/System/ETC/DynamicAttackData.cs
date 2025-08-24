using DadVSMe.Entities;
using UnityEngine;

namespace DadVSMe
{
    public class DynamicAttackData : IAttackData
    {
        private int damage;
        public int Damage => damage;

        public EAttackFeedback AttackFeedback => staticAttackData.AttackFeedback;

        public AttackDataBase staticAttackData;

        public DynamicAttackData(AttackDataBase staticAttackData)
        {
            this.staticAttackData = staticAttackData;
            this.damage = staticAttackData.Damage;
        }

        public void SetDamage(int damage)
        {
            this.damage = damage;
        }

        public FeedbackData GetFeedbackData(EAttackAttribute attackAttribute)
        {
            return staticAttackData.GetFeedbackData(attackAttribute);
        }
    }
}