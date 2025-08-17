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

        public void Initialize(UnitStat hpStat)
        {
            this.maxHP = (int)hpStat.FinalValue;
            
            currentHP = maxHP;

            hpStat.onStatChanged.AddListener(OnHealthStatChanged);
        }

        public void Attack(Unit attacker, IAttackData attackData)
        {
            currentHP -= (int)(attackData.Damage * attacker.UnitData.Stat[EUnitStat.AttackPowerMultiplier].FinalValue);
            onAttackEvent.Invoke(attacker, attackData);
        }

        public void Heal(int amount)
        {
            currentHP += amount;
            currentHP = Mathf.Min(currentHP, maxHP);
        }

        private void OnHealthStatChanged(float value)
        {
            maxHP = (int)value;
        }
    }
}