using System;
using DadVSMe.Core.UI;
using H00N.AI.FSM;
using UnityEngine;
using UnityEngine.Events;

namespace DadVSMe.Entities
{
    public class UnitHealth : MonoBehaviour
    {
        public UnityEvent<Unit, IAttackData> onAttackEvent = null;
        public event Action OnHPChangedEvent = null;

        private UnitStat hpStat = null;

        private int currentHP;
        public int CurrentHP => currentHP;

        public void Initialize(UnitStat hpStat)
        {
            this.hpStat = hpStat;
            currentHP = (int)hpStat.FinalValue;
        }

        public void Attack(Unit attacker, IAttackData attackData)
        {
            Debug.Log((attackData));
            currentHP -= (int)attackData.Damage;
            onAttackEvent?.Invoke(attacker, attackData);
            OnHPChangedEvent?.Invoke();

            var handle =
                UIManager.CreateUIHandle<DamageTextUIHandlse, DamageTextUIHandleParameter>(out DamageTextUIHandleParameter param);
            param.target = transform;
            param.attackAttribute = attacker.GetComponent<FSMBrain>().GetAIData<UnitFSMData>().attackAttribute;
            param.attackData = attackData;
            param.upOffset = Vector3.up;
            param.damage = attackData.Damage * attacker.Stat[EUnitStat.AttackPowerMultiplier].FinalValue;
            handle.Execute(param);
        }

        public void Heal(int amount)
        {
            currentHP += amount;
            currentHP = Mathf.Min(currentHP, (int)hpStat.FinalValue);
            OnHPChangedEvent?.Invoke();
        }
    }
}