using DadVSMe.Entities;
using UnityEngine;

namespace DadVSMe
{
    [CreateAssetMenu(fileName = "DeadBombSkillData", menuName = "DadVSMe/SkillData/Data/DeadBombSkillData")]
    public class DeadBombSkillData : SkillData<DeadBombSkill, DeadBombSkillData.Option>
    {
        [System.Serializable]
        public class Option : SkillOption
        {
            [Range(0f, 1f)] public float chance;
            public int damage;
        }

        public AttackDataBase attackData;
        public float attackRadius;
    }
}
