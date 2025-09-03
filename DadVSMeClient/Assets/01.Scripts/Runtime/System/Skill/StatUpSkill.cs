using DadVSMe.Entities;

namespace DadVSMe
{
    public abstract class StatUpSkill<TData, TOption> : UnitSkill<TData, TOption> where TData : SkillDataBase where TOption : SkillOption
    {
        protected UnitStatData statData = null;

        public override void OnRegist(UnitSkillComponent ownerComponent, SkillDataBase skillData)
        {
            statData = ownerComponent.GetComponent<Unit>().FSMBrain.GetAIData<UnitStatData>();
            base.OnRegist(ownerComponent, skillData);
            Execute();
        }

        protected override void OnLevelUp()
        {
            base.OnLevelUp();
            Execute();
        }
    }
}
