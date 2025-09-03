using DadVSMe.Entities;
using UnityEngine;

namespace DadVSMe
{
    [CreateAssetMenu(fileName = "BowlingSkillData", menuName = "DadVSMe/SkillData/Data/BowlingSkillData")]
    public class BowlingSkillData : SkillData<BowlingSkill, BowlingSkillData.Option>
    {
        [System.Serializable]
        public class Option : SkillOption
        {
            public int damage;
        }

        public AttackDataBase bowlingHitAttackData;
    }
}
