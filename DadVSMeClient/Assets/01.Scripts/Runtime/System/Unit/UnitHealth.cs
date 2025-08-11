
using UnityEngine;
using UnityEngine.Events;

namespace DadVSMe.Entities
{
    public class UnitHealth : MonoBehaviour
    {
        [SerializeField] UnityEvent<Unit, IAttackData> onAttackEvent = null;

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

        public void Attack(Unit attacker, IAttackData attackData)
        {
            currentHP -= attackData.Damage;
            onAttackEvent.Invoke(attacker, attackData);
        }

        public void Heal(int amount)
        {
            currentHP += amount;
            currentHP = Mathf.Min(currentHP, maxHP);
        }
    }
}