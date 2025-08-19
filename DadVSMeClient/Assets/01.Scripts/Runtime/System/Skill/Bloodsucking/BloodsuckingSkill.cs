using DadVSMe.Entities;
using UnityEngine;

namespace DadVSMe
{
    public class BloodsuckingSkill : UnitSkill
    {
        private UnitHealth health;

        private float healRatio;
        private float healRatioIncreaseRate;

        public BloodsuckingSkill(float healRatio, float healRatioIncreaseRate)
        {
            this.healRatio = healRatio;
            this.healRatioIncreaseRate = healRatioIncreaseRate;
        }

        public override void OnRegist(UnitSkillComponent ownerComponent)
        {
            base.OnRegist(ownerComponent);

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
            owner.onAttackTargetEvent.AddListener(OnAttackTarget);
        }

        private void OnAttackTarget(Unit target, IAttackData data)
        {
            health.Heal((int)(data.Damage * healRatio));
        }

        public override void LevelUp()
        {
            base.LevelUp();

            healRatio += healRatioIncreaseRate;
        }
    }
}