using DadVSMe.Entities;
using DadVSMe.Players;

namespace DadVSMe
{
    public class ItemMagnetSkill : StatUpSkill
    {
        private float levelUpIncreaseRate;
        private UnitStatData statData;

        public ItemMagnetSkill(float cooltime, float checkRadius, float levelUpIncreaseRate, float magnetSpeedMultiplier) : base()
        {
            this.levelUpIncreaseRate = levelUpIncreaseRate;
        }

        public override void OnRegist(UnitSkillComponent ownerComponent)
        {
            statData = ownerComponent.GetComponent<Player>().FSMBrain.GetAIData<UnitStatData>();
            base.OnRegist(ownerComponent);
        }

        public override void Execute()
        {
            base.Execute();

            statData[EUnitStat.ItemMagnetRadius].RegistAddModifier(levelUpIncreaseRate);
        }   
    }
}