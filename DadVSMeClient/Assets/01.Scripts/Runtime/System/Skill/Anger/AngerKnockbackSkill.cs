using DadVSMe.Entities;
using H00N.AI.FSM;
using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe
{
    public class AngerKnockbackSkill : UnitSkill
    {
        private Unit owner = null;
        private AttackDataBase attackData;
        private float knockbackRange;
        private float levelUpIncreaseRate;

        public AngerKnockbackSkill(AttackDataBase attackData, float knockbackRange, float levelUpIncreaseRate) : base()
        {
            this.attackData = attackData;
            this.knockbackRange = knockbackRange;
            this.levelUpIncreaseRate = levelUpIncreaseRate;
        }

        public override void OnRegist(UnitSkillComponent ownerComponent)
        {
            base.OnRegist(ownerComponent);
            owner = ownerComponent.GetComponent<Unit>();
            owner.FSMBrain.OnStateChangedEvent.AddListener(OnStatChanged);
        }

        public override void Execute()
        {
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

                if (col.gameObject.TryGetComponent<UnitHealth>(out UnitHealth targetHealth))
                {
                    targetHealth.Attack(ownerComponent.GetComponent<Unit>(), attackData);
                    _ = new PlayHitFeedback(attackData, attackAttribute, targetHealth.transform.position, Vector3.zero, unitFSMData.forwardDirection);
                }
            }

            unitFSMData.attackAttribute = attackAttribute;
        }

        public override void OnUnregist()
        {
            base.OnUnregist();
            ownerComponent.GetComponent<FSMBrain>().OnStateChangedEvent.RemoveListener(OnStatChanged);
        }

        public override void LevelUp()
        {
            base.LevelUp();

            knockbackRange = levelUpIncreaseRate;
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
