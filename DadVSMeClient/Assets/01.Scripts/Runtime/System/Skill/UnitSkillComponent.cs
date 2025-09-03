using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace DadVSMe
{
    public class UnitSkillComponent : MonoBehaviour, IEnumerable<SkillType>
    {
        [SerializeField] private SkillDataContainer skillDataContainer;
        public SkillDataContainer SkillDataContainer => skillDataContainer;

        private Dictionary<SkillType, UnitSkillBase> skillContainer;
        public Dictionary<SkillType, UnitSkillBase> SkillContainer => skillContainer;
        public event Action<SkillType> OnSkillChangedEvent;

        public virtual void Initialize()
        {
            skillContainer = new();

            // RegistSkill(SkillType.ItemMagnet);
            // RegistSkill(SkillType.GuidedOrb);
            // RegistSkill(SkillType.GuidedOrb);
            // RegistSkill(SkillType.GuidedOrb);
            // RegistSkill(SkillType.GuidedOrb);
            // RegistSkill(SkillType.GuidedOrb);
            // RegistSkill(SkillType.AttackBlast);
            // RegistSkill(SkillType.FirePunch);
            // RegistSkill(SkillType.StatikkShiv);
            // RegistSkill(SkillType.Bowling);
        }

        public void RegistSkill(SkillType skillType)
        {
            if (skillContainer.TryGetValue(skillType, out UnitSkillBase unitSkill))
            {
                unitSkill.LevelUp();
            }
            else
            {
                UnitSkillBase newSkill = skillDataContainer.CreateSkill(skillType);
                skillContainer[skillType] = newSkill;
                newSkill.OnRegist(this, skillDataContainer.GetSkillData(skillType));
            }

            OnSkillChangedEvent?.Invoke(skillType);
        }

        public void UnregistSkill(SkillType skillType)
        {
            if (skillContainer.TryGetValue(skillType, out UnitSkillBase unitSkill))
            {
                unitSkill.OnUnregist();
                skillContainer.Remove(skillType);
            }
            else
            {
            }

            OnSkillChangedEvent?.Invoke(skillType);
        }

        public UnitSkillBase GetSkill(SkillType skillType)
        {
            skillContainer.TryGetValue(skillType, out UnitSkillBase unitSkill);
            return unitSkill;
        }

        IEnumerator<SkillType> IEnumerable<SkillType>.GetEnumerator() => skillContainer.Keys.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => skillContainer.Keys.GetEnumerator();
    }
}
