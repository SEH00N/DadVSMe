using System.Collections.Generic;
using UnityEngine;

namespace DadVSMe
{
    public class UnitSkillComponent : MonoBehaviour
    {
        [SerializeField] private SkillDataContainer skillDataContainer;
        public SkillDataContainer SkillDataContainer => skillDataContainer;

        private Dictionary<SkillType, UnitSkill> skillContainer;

        public virtual void Initialize()
        {
            skillContainer = new();
        }

        public void RegistSkill(SkillType skillType)
        {
            if (skillContainer.TryGetValue(skillType, out UnitSkill unitSkill))
            {
                unitSkill.LevelUp();
            }
            else
            {
                UnitSkill newSkill = skillDataContainer.CreateSkill(skillType);
                skillContainer[skillType] = newSkill;
                newSkill.OnRegist(this);
            }
        }

        public void UnregistSkill(SkillType skillType)
        {
            if (skillContainer.TryGetValue(skillType, out UnitSkill unitSkill))
            {
                unitSkill.OnUnregist();
                skillContainer.Remove(skillType);
            }
            else
            {

            }
        }

        public UnitSkill GetSkill(SkillType skillType)
        {
            skillContainer.TryGetValue(skillType, out UnitSkill unitSkill);
            return unitSkill;
        }
    }
}
