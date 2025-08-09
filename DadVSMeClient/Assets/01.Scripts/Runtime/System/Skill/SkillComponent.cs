using System;
using System.Collections.Generic;
using UnityEngine;

namespace DadVSMe
{
    public class SkillComponent : MonoBehaviour
    {
        private Dictionary<Type, Skill> skillContainer;

        public void RegistSkill<T>(T skill) where T : Skill
        {
            Type skillType = skill.GetType();

            if (skillContainer.ContainsKey(skillType))
            {
                skillContainer[skillType].LevelUp();
            }
            else
            {
                skillContainer.Add(skillType, skill);
                skill.OnRegist(gameObject);
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
