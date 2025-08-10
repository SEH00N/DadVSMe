using UnityEngine;

namespace DadVSMe
{
    public class StatUpSkill : PassiveSkill
    {
        protected float StatUpRate = 1f;

        public override void OnRegist(UnitSkillComponent ownerComponent)
        {
            base.OnRegist(ownerComponent);

            Execute();
        }

        public override void LevelUp()
        {
            base.LevelUp();

            Execute();
        }

        //임의 수식. 나중에 테이블 만들어서 가져오든 해야할듯
        public float StatUpAmount()
        {
            return /*target value*/ - ((level - 1) * StatUpRate) + level * StatUpRate;
        }
    }
}
