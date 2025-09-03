using UnityEngine;

namespace DadVSMe
{
    [CreateAssetMenu(fileName = "ItemMagnetSkillData", menuName = "DadVSMe/SkillData/Data/ItemMagnetSkillData")]
    public class ItemMagnetSkillData : SkillData<ItemMagnetSkill, ItemMagnetSkillData.Option>
    {
        [System.Serializable]
        public class Option : SkillOption
        {
            public float additiveValue;
        }
    }
}