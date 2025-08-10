using System;
using System.Collections.Generic;
using UnityEngine;

namespace DadVSMe
{
    public class UnitSkillComponent : MonoBehaviour
    {
        private Dictionary<Type, UnitSkill> skillContainer;

        public void RegistSkill<T>(T skill) where T : UnitSkill
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
