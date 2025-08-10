using System;
using System.Collections.Generic;
using H00N.Resources.Addressables;
using UnityEngine;

namespace DadVSMe
{
    public class UnitSkillComponent : MonoBehaviour
    {
        [SerializeField] AddressableAsset<AttackBlast> prefab = null;
        private Dictionary<Type, UnitSkill> skillContainer;

        public async virtual void Initialize()
        {
            skillContainer = new();

            await prefab.InitializeAsync();
            RegistSkill<AttackBlastSkill>(new AttackBlastSkill(prefab));
        }

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
