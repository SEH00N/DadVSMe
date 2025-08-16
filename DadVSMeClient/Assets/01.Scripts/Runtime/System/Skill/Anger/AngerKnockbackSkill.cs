using DadVSMe.Entities;
using H00N.AI.FSM;
using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe
{
    public class AngerKnockbackSkill : UnitSkill
    {
        private AttackDataBase attackData;

        public AngerKnockbackSkill(AttackDataBase attackData) : base()
        {
            this.attackData = attackData;
        }

        public override void OnRegist(UnitSkillComponent ownerComponent)
        {
            base.OnRegist(ownerComponent);
            ownerComponent.GetComponent<FSMBrain>().OnStateChangedEvent.AddListener(OnStatChanged);
        }

        public override void Execute()
        {
            Collider2D[] cols = Physics2D.OverlapCircleAll(ownerComponent.transform.position, 10f);

            if (cols.Length == 0)
                return;

            foreach (var col in cols)
            {
                if (col.gameObject == ownerComponent.gameObject)
                    continue;

                if (col.gameObject.TryGetComponent<UnitHealth>(out UnitHealth targetHealth))
                {
                    targetHealth.Attack(ownerComponent.GetComponent<Unit>(), attackData);
                }
            }
        }

        public override void OnUnregist()
        {
            base.OnUnregist();
            ownerComponent.GetComponent<FSMBrain>().OnStateChangedEvent.RemoveListener(OnStatChanged);
        }

        private void OnStatChanged(FSMState current, FSMState target)
        {
            if (target.gameObject.name == "PowerUp")
            {
                Execute();
            }
        }
    }
}
