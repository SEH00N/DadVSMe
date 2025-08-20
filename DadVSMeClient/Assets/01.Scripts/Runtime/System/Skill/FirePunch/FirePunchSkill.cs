using DadVSMe.Entities;
using UnityEngine;

namespace DadVSMe
{
    public class FirePunchSkill : UnitSkill
    {
        private float levelUpIncreaseRate;

        public FirePunchSkill(float levelUpIncreaseRate) : base()
        {
            this.levelUpIncreaseRate = levelUpIncreaseRate;
        }

        public override void OnRegist(UnitSkillComponent ownerComponent)
        {
            base.OnRegist(ownerComponent);

            Execute();
        }

        public override void Execute()
        {
            ownerComponent.GetComponent<Unit>().SetAttackAttribute(EAttackAttribute.Fire);
        }

        public override void OnUnregist()
        {
            base.OnUnregist();

            ownerComponent.GetComponent<Unit>().SetAttackAttribute(EAttackAttribute.Normal);
        }

        public override void LevelUp()
        {
            base.LevelUp();

            Unit owner = ownerComponent.GetComponent<Unit>();
            UnitStatData ownerStatData = owner.FSMBrain.GetAIData<UnitStatData>();
            UnitStat AttackPowerMultiplierStat = ownerStatData[EUnitStat.AttackPowerMultiplier];
            AttackPowerMultiplierStat.RegistAddModifier(levelUpIncreaseRate);
        }
    }
}