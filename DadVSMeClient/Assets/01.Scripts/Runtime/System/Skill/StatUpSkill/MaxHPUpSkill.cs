using DadVSMe.Entities;

namespace DadVSMe
{
    public class MaxHPUpSkill : StatUpSkill
    {
        public MaxHPUpSkill() : base()
        {
            StatUpRate = 30f;
        }

        public override void Execute()
        {
            base.Execute();

            Unit owner = ownerComponent.GetComponent<Unit>();
            UnitStatData ownerStatData = owner.FSMBrain.GetAIData<UnitStatData>();
            UnitStat hpStat = ownerStatData[EUnitStat.MaxHp];
            hpStat.RegistAddModifier(StatUpAmount()); //임의 수식. 나중에 테이블 만들어서 가져오든 해야할듯
        }
    }
}
