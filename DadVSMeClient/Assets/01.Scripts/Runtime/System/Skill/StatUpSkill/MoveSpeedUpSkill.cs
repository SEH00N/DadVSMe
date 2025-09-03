namespace DadVSMe
{
    public class MoveSpeedUpSkill : StatUpSkill<MoveSpeedUpSkillData, MoveSpeedUpSkillData.Option>
    {
        public override void Execute()
        {
            statData[EUnitStat.MoveSpeed].RegistAddModifier(GetOption().additiveValue);
        }
    }
}
