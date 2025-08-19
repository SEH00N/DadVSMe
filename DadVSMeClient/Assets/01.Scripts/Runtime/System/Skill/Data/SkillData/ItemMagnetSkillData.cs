using UnityEngine;

namespace DadVSMe
{
    [CreateAssetMenu(fileName = "ItemMagnetSkillData", menuName = "DadVSMe/SkillData/Data/ItemMagnetSkillData")]
    public class ItemMagnetSkillData : SkillData
    {
        public float checkRadius;
        public float levelUpIncreaseRate;
        public float magnetSpeedMultiplier;
        
        public override UnitSkill CreateSkill()
        {
            return new ItemMagnetSkill(Time.deltaTime, checkRadius, levelUpIncreaseRate, magnetSpeedMultiplier);
        }
    }
}