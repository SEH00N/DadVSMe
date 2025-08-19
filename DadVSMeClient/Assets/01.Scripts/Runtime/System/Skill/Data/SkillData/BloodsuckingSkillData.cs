using UnityEngine;

namespace DadVSMe
{
    [CreateAssetMenu(fileName = "BloodsuckingSkillData", menuName = "DadVSMe/SkillData/Data/BloodsuckingSkillData")]
    public class BloodsuckingSkillData : SkillData
    {
        public float healRatio;
        public float healRatioIncreaseRate;

        public override UnitSkill CreateSkill()
        {
            return new BloodsuckingSkill(healRatio, healRatioIncreaseRate);
        }
    }
}
