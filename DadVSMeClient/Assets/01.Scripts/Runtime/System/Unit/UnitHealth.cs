using System;
using UnityEngine;
using UnityEngine.Events;

namespace DadVSMe.Entities
{
    public class UnitHealth : MonoBehaviour, IHealth
    {
        public Vector3 Position => transform.position;

        public UnityEvent<IAttacker, IAttackData> onAttackEvent = null;
        public event Action OnHPChangedEvent = null;

        private UnitStat hpStat = null;

        private int currentHP;
        public int CurrentHP => currentHP;

        public void Initialize(UnitStat hpStat)
        {
            this.hpStat = hpStat;
            currentHP = (int)hpStat.FinalValue;
        }

        public void Attack(IAttacker attacker, IAttackData attackData)
        {
            currentHP -= (int)attackData.Damage;
            onAttackEvent?.Invoke(attacker, attackData);
            OnHPChangedEvent?.Invoke();
        }

        public void Heal(int amount)
        {
            currentHP += amount;
            currentHP = Mathf.Min(currentHP, (int)hpStat.FinalValue);
            OnHPChangedEvent?.Invoke();
        }
    }
}