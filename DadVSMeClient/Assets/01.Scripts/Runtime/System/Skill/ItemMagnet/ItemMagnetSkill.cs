namespace DadVSMe
{
    public class ItemMagnetSkill : StatUpSkill<ItemMagnetSkillData, ItemMagnetSkillData.Option>
    {
        public override void Execute()
        {
            statData[EUnitStat.ItemMagnetRadius].RegistAddModifier(GetOption().additiveValue);
        }
    }
}