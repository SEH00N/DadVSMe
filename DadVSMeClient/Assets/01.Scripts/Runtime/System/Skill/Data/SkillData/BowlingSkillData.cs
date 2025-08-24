using DadVSMe.Entities;
using UnityEngine;

namespace DadVSMe
{
    [CreateAssetMenu(fileName = "BowlingSkillData", menuName = "DadVSMe/SkillData/Data/BowlingSkillData")]
    public class BowlingSkillData : SkillData
    {
        public float levelUpIncreaseRate;
        public AttackDataBase bowlingHitAttackData;
    
        public override UnitSkill CreateSkill()
        {
            return new BowlingSkill(this);
        }
    }
}
