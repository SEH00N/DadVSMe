using DadVSMe.Entities;
using UnityEngine;

namespace DadVSMe
{
    [CreateAssetMenu(fileName = "AngerKnockbackSkillData", menuName = "DadVSMe/SkillData/Data/AngerKnockbackSkillData")]
    public class AngerKnockbackSkillData : SkillData<AngerKnockbackSkill, AngerKnockbackSkillData.Option>
    {
        [System.Serializable]
        public class Option : SkillOption
        {
            public float knockbackRange;
            public int damage;
        }

        public AttackDataBase attackData;
    }
}
