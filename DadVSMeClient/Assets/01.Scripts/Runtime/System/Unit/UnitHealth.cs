using System;
using UnityEngine;
using UnityEngine.Events;

namespace DadVSMe.Entities
{
    public class UnitHealth : MonoBehaviour, IHealth
    {
        [SerializeField] float invincibilityTime = 0f;

        public Vector3 Position => transform.position;

        public UnityEvent<IAttacker, IAttackData> onAttackEvent = null;
        public event Action OnHPChangedEvent = null;

        private UnitStat hpStat = null;

        private int currentHP;
        public int CurrentHP => currentHP;

        private float lastAttackTime = 0f;

        public void Initialize(UnitStat hpStat)
        {
            this.hpStat = hpStat;
            currentHP = (int)hpStat.FinalValue;
        }

        public void Attack(IAttacker attacker, IAttackData attackData)
        {
            if(Time.time - lastAttackTime < invincibilityTime)
                return;

            lastAttackTime = Time.time;

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