using DadVSMe.Entities;
using UnityEngine;

namespace DadVSMe
{
    public class DynamicAttackData : IAttackData, IAttackFeedbackDataContainer, IJuggleAttackData
    {
        private int damage;
        public int Damage => damage;

        public EAttackFeedback AttackFeedback => staticAttackData.AttackFeedback;

        public float JuggleForce => (staticAttackData as IJuggleAttackData)?.JuggleForce ?? 0f;
        public Vector2 JuggleDirection => (staticAttackData as IJuggleAttackData)?.JuggleDirection ?? Vector2.zero;

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