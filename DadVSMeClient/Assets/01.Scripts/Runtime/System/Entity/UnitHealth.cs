
using UnityEngine;
using UnityEngine.Events;

namespace DadVSMe.Entities
{
    public class UnitHealth : MonoBehaviour
    {
        [SerializeField] UnityEvent<EAttackFeedback, float> onAttackEvent = null;

        private int maxHP;
        public int MaxHP
        {
            get => maxHP;

            set
            {
                maxHP = value;
            }
        }

        private int currentHP;
        public int CurrentHP => currentHP;

        public void Initialize(int maxHP)
        {
            this.maxHP = maxHP;
            currentHP = maxHP;
        }

        public void Attack(int damage, EAttackFeedback feedback, float feedbackValue)
        {
            currentHP -= damage;
            onAttackEvent.Invoke(feedback, feedbackValue);
        }

        public void Heal(int amount)
        {
            currentHP += amount;
            currentHP = Mathf.Min(currentHP, maxHP);
        }
    }
}