using UnityEngine;

namespace DadVSMe
{
    [CreateAssetMenu(fileName = "MaxHpUpSkillData", menuName = "DadVSMe/SkillData/Data/MaxHpUpSkillData")]
    public class MaxHpUpSkillData : SkillData
    {
        public override UnitSkill CreateSkill()
        {
            return new MaxHPUpSkill();
        }
    }
}
