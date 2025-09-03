using UnityEngine;

namespace DadVSMe
{
    [CreateAssetMenu(fileName = "HealPackSkillData", menuName = "DadVSMe/SkillData/Data/HealPackSkillData")]
    public sealed class HealPackSkillData : SkillDataBase
    {
        public int healAmount;

        public override UnitSkillBase CreateSkill() => null;
        public override T GetOption<T>(int level) => null;
    }
}
