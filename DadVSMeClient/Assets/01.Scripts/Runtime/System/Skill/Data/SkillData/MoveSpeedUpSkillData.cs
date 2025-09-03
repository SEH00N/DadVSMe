using UnityEngine;

namespace DadVSMe
{
    [CreateAssetMenu(fileName = "MoveSpeedUpSkillData", menuName = "DadVSMe/SkillData/Data/MoveSpeedUpSkillData")]
    public class MoveSpeedUpSkillData : SkillData<MoveSpeedUpSkill, MoveSpeedUpSkillData.Option>
    {
        [System.Serializable]
        public class Option : SkillOption
        {
            public float additiveValue;
        }
    }
}
