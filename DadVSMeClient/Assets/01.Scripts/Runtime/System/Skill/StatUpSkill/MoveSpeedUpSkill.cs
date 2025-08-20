using DadVSMe.Entities;

namespace DadVSMe
{
    public class MoveSpeedUpSkill : StatUpSkill
    {
        private float levelUpIncreaseRate;

        public MoveSpeedUpSkill(float levelUpIncreaseRate) : base()
        {
            this.levelUpIncreaseRate = levelUpIncreaseRate;
        }

        public override void Execute()
        {
            base.Execute();

            Unit owner = ownerComponent.GetComponent<Unit>();
            UnitStatData ownerStatData = owner.FSMBrain.GetAIData<UnitStatData>();
            UnitStat moveSpeedStat = ownerStatData[EUnitStat.MoveSpeed];
            moveSpeedStat.RegistAddModifier(levelUpIncreaseRate);
        }
    }
}
