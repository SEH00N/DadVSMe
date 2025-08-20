using DadVSMe.Entities;

namespace DadVSMe
{
    public class MaxHPUpSkill : StatUpSkill
    {
        public float levelUpIncreaseRate;

        public MaxHPUpSkill(float levelUpIncreaseRate) : base()
        {
            this.levelUpIncreaseRate = levelUpIncreaseRate;
        }

        public override void Execute()
        {
            base.Execute();

            Unit owner = ownerComponent.GetComponent<Unit>();
            UnitStatData ownerStatData = owner.FSMBrain.GetAIData<UnitStatData>();
            UnitStat hpStat = ownerStatData[EUnitStat.MaxHp];
            hpStat.RegistAddModifier(levelUpIncreaseRate);
        }
    }
}
