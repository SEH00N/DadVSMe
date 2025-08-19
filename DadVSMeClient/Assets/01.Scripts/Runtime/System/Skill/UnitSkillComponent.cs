using System;
using System.Collections.Generic;
using DadVSMe.Entities;
using H00N.Resources.Addressables;
using UnityEngine;

namespace DadVSMe
{
    public class UnitSkillComponent : MonoBehaviour
    {
        [SerializeField] private SkillDataContainer skillDataContainer;
        public SkillDataContainer SkillDataContainer => skillDataContainer;

        private Dictionary<Type, UnitSkill> skillContainer;

        public virtual void Initialize()
        {
            skillContainer = new();
        }

        public void RegistSkill(UnitSkill skill)
        {
            Type skillType = skill.GetType();

            if (skillContainer.ContainsKey(skillType))
            {
                skillContainer[skillType].LevelUp();
            }
            else
            {
                skillContainer.Add(skillType, skill);
                skill.OnRegist(this);
            }
        }

        public void UnregistSkill(Type skillType)
        {
            if (skillContainer.ContainsKey(skillType))
            {
                skillContainer[skillType].OnUnregist();
                skillContainer.Remove(skillType);
            }
            else
            {

            }
        }
    }
}
