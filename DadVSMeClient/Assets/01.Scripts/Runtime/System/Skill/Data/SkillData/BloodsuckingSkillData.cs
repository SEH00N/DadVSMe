using UnityEngine;

namespace DadVSMe
{
    [CreateAssetMenu(fileName = "BloodsuckingSkillData", menuName = "DadVSMe/SkillData/Data/BloodsuckingSkillData")]
    public class BloodsuckingSkillData : SkillData<BloodsuckingSkill, BloodsuckingSkillData.Option>
    {
        [System.Serializable]
        public class Option : SkillOption
        {
            public float healRatio;
        }
    }
}
