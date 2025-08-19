using DadVSMe.Entities;
using UnityEngine;

namespace DadVSMe
{
    [CreateAssetMenu(fileName = "AngerKnockbackSkillData", menuName = "DadVSMe/SkillData/Data/AngerKnockbackSkillData")]
    public class AngerKnockbackSkillData : SkillData
    {
        public AttackDataBase attackData;
        public float knockbackRange;

        public override UnitSkill CreateSkill()
        {
            return new AngerKnockbackSkill(attackData, knockbackRange);
        }
    }
}
