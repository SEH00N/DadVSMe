using System;
using System.Collections.Generic;
using DadVSMe.Entities;
using H00N.Resources.Addressables;
using UnityEngine;

namespace DadVSMe
{
    public class UnitSkillComponent : MonoBehaviour
    {
        [SerializeField] AddressableAsset<AttackBlast> attackBlastPrefab = null;
        [SerializeField] AddressableAsset<GuidedOrb> guidedOrbPrefab = null;
        [SerializeField] AddressableAsset<StatikkShivLighting> statikkShivLightingPrefab = null;
        [SerializeField] AttackDataBase attackData;
        private Dictionary<Type, UnitSkill> skillContainer;


        public virtual void Initialize()
        {
            skillContainer = new();

            //await statikkShivLightingPrefab.InitializeAsync();
            RegistSkill<FirePunchSkill>(new FirePunchSkill());
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
