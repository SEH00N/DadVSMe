using UnityEngine;

namespace DadVSMe
{
    [CreateAssetMenu(fileName = "MaxHpUpSkillData", menuName = "DadVSMe/SkillData/Data/MaxHpUpSkillData")]
    public class MaxHpUpSkillData : SkillData
    {
        public float levelUpIncreaseRate;

        public override UnitSkill CreateSkill()
        {
            return new MaxHPUpSkill(levelUpIncreaseRate);
        }
    }
}
