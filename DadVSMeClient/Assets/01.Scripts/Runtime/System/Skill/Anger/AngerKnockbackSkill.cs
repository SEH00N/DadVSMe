using DadVSMe.Entities;
using H00N.AI.FSM;
using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe
{
    public class AngerKnockbackSkill : UnitSkill<AngerKnockbackSkillData, AngerKnockbackSkillData.Option>
    {
        private Unit owner = null;

        public override void OnRegist(UnitSkillComponent ownerComponent, SkillDataBase skillData)
        {
            base.OnRegist(ownerComponent, skillData);

            _ = new InitializeAttackFeedback(GetData().attackData);

            owner = ownerComponent.GetComponent<Unit>();
            owner.FSMBrain.OnStateChangedEvent.AddListener(OnStatChanged);
        }

        public override void Execute()
        {
            float knockbackRange = GetOption().knockbackRange;
            AttackDataBase attackData = GetData().attackData;

            Collider2D[] cols = Physics2D.OverlapCircleAll(ownerComponent.transform.position, knockbackRange);

            if (cols.Length == 0)
                return;

            UnitFSMData unitFSMData = owner.FSMBrain.GetAIData<UnitFSMData>();
            EAttackAttribute attackAttribute = unitFSMData.attackAttribute;
            unitFSMData.attackAttribute = EAttackAttribute.Crazy;

            foreach (var col in cols)
            {
                if (col.gameObject == ownerComponent.gameObject)
                    continue;

                if (col.gameObject.TryGetComponent<IHealth>(out IHealth targetHealth))
                {
                    targetHealth.Attack(ownerComponent.GetComponent<Unit>(), attackData);
                    _ = new PlayHitFeedback(attackData, unitFSMData.attackAttribute, targetHealth.Position, Vector3.zero, unitFSMData.forwardDirection);
                }
            }

            _ = new PlayAttackFeedback(attackData, unitFSMData.attackAttribute, ownerComponent.transform.position, Vector3.zero, unitFSMData.forwardDirection);
            _ = new PlayAttackSound(attackData, unitFSMData.attackAttribute);

            unitFSMData.attackAttribute = attackAttribute;
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
