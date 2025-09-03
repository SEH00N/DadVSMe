using DadVSMe.Entities;
using UnityEngine;

namespace DadVSMe
{
    public class BloodsuckingSkill : UnitSkill<BloodsuckingSkillData, BloodsuckingSkillData.Option>
    {
        private UnitHealth health;

        public override void OnRegist(UnitSkillComponent ownerComponent, SkillDataBase skillData)
        {
            base.OnRegist(ownerComponent, skillData);

            Unit owner = ownerComponent.GetComponent<Unit>();
            owner.onAttackTargetEvent.AddListener(OnAttackTarget);

            health = owner.GetComponent<UnitHealth>();
        }

        public override void Execute()
        {

        }

        public override void OnUnregist()
        {
            base.OnUnregist();

            Unit owner = ownerComponent.GetComponent<Unit>();
            owner.onAttackTargetEvent.RemoveListener(OnAttackTarget);
        }

        private void OnAttackTarget(Unit target, IAttackData data)
        {
            health.Heal((int)(data.Damage * GetOption().healRatio));
        }
    }
}