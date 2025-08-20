using UnityEngine;

namespace DadVSMe
{
    [CreateAssetMenu(fileName = "MoveSpeedUpSkillData", menuName = "DadVSMe/SkillData/Data/MoveSpeedUpSkillData")]
    public class MoveSpeedUpSkillData : SkillData
    {
        public float levelUpIncreaseRate;

        public override UnitSkill CreateSkill()
        {
            return new MoveSpeedUpSkill(levelUpIncreaseRate);
        }
    }
}
