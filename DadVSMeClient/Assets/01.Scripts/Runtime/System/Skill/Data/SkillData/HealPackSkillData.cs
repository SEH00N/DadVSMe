using UnityEngine;

namespace DadVSMe
{
    [CreateAssetMenu(fileName = "HealPackSkillData", menuName = "DadVSMe/SkillData/Data/HealPackSkillData")]
    public class HealPackSkillData : SkillData
    {
        public int healAmount;
        public override UnitSkill CreateSkill() { return null; }
    }
}
