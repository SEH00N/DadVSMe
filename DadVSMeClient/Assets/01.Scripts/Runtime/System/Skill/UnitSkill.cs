namespace DadVSMe
{
    public abstract class UnitSkillBase
    {
        protected SkillDataBase skillData;

        protected UnitSkillComponent ownerComponent;
        protected int level;
        public int Level => level;

        public UnitSkillBase()
        {
            ownerComponent = null;
            skillData = null;
            level = 1;
        }

        public virtual void OnRegist(UnitSkillComponent ownerComponent, SkillDataBase skillData)
        {
            this.ownerComponent = ownerComponent;
            this.skillData = skillData;
        }

        public virtual void OnUnregist() { }
        public abstract void Execute();

        protected virtual void OnLevelUp() { }
        public void LevelUp()
        {
            level++;
            OnLevelUp();
        }
    }

    public abstract class UnitSkill<TData, TOption> : UnitSkillBase where TData : SkillDataBase where TOption : SkillOption
    {
        protected TData GetData() => skillData as TData;
        protected TOption GetOption() => skillData.GetOption<TOption>(level);
    }
}