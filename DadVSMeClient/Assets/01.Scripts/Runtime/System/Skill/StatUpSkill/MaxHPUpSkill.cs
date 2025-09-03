namespace DadVSMe
{
    public class MaxHPUpSkill : StatUpSkill<MaxHpUpSkillData, MaxHpUpSkillData.Option>
    {
        public override void Execute()
        {
            statData[EUnitStat.MaxHp].RegistAddModifier(GetOption().additiveValue);
        }
    }
}
