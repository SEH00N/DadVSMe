using DadVSMe.Entities;
using H00N.AI.FSM;
using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe
{
    public class AngerKnockbackSkill : UnitSkill
    {
        private AttackDataBase attackData;
        private float knockbackRange;

        public AngerKnockbackSkill(AttackDataBase attackData, float knockbackRange = 10) : base()
        {
            this.attackData = attackData;
            this.knockbackRange = knockbackRange;
        }

        public override void OnRegist(UnitSkillComponent ownerComponent)
        {
            base.OnRegist(ownerComponent);
            ownerComponent.GetComponent<FSMBrain>().OnStateChangedEvent.AddListener(OnStatChanged);
        }

        public override void Execute()
        {
            Collider2D[] cols = Physics2D.OverlapCircleAll(ownerComponent.transform.position, knockbackRange);

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

        public override void LevelUp()
        {
            base.LevelUp();

            knockbackRange = (knockbackRange - (level - 1)) + level;
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
