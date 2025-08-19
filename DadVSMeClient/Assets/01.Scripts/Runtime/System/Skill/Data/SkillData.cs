using UnityEngine;

namespace DadVSMe
{
    public abstract class SkillData : ScriptableObject
    {
        public SkillType skillType;

        public abstract UnitSkill CreateSkill();
    }
}
