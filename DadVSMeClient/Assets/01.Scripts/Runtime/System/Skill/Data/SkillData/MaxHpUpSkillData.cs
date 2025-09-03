using UnityEngine;

namespace DadVSMe
{
    [CreateAssetMenu(fileName = "MaxHpUpSkillData", menuName = "DadVSMe/SkillData/Data/MaxHpUpSkillData")]
    public class MaxHpUpSkillData : SkillData<MaxHPUpSkill, MaxHpUpSkillData.Option>
    {
        [System.Serializable]
        public class Option : SkillOption
        {
            public float additiveValue;
        }
    }
}
