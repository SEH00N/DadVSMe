using UnityEngine;

namespace DadVSMe
{
    [CreateAssetMenu(fileName = "FirePunchSkillData", menuName = "DadVSMe/SkillData/Data/FirePunchSkillData")]
    public class FirePunchSkillData : SkillData
    {
        public override UnitSkill CreateSkill()
        {
            return new FirePunchSkill();
        }
    }
}
