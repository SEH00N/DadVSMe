using System.Collections.Generic;
using UnityEngine;

namespace DadVSMe.UI.Skills
{
    public class SkillInfoUI : MonoBehaviourUI
    {
        [SerializeField] List<SkillInfoElementUI> elementUIList = null;
        private UnitSkillComponent unitSkillComponent;

        public void Initialize(UnitSkillComponent unitSkillComponent)
        {
            this.unitSkillComponent = unitSkillComponent;
            
            int i = 0;
            foreach(SkillType skillType in unitSkillComponent)
            {
                SkillDataBase skillData = unitSkillComponent.SkillDataContainer.GetSkillData(skillType);
                UnitSkillBase unitSkill = unitSkillComponent.GetSkill(skillType);
                elementUIList[i].Initialize(skillData, unitSkill?.Level ?? 0);
                i++;
            }

            for(; i < elementUIList.Count; i++)
                elementUIList[i].Initialize(null, 0);

            unitSkillComponent.OnSkillChangedEvent += HandleSkillChanged;
        }

        private void HandleSkillChanged(SkillType skillType)
        {
            SkillDataBase skillData = unitSkillComponent.SkillDataContainer.GetSkillData(skillType);
            for(int i = 0; i < elementUIList.Count; i++)
            {
                SkillInfoElementUI elementUI = elementUIList[i];
                if(elementUI.SkillData != null && elementUI.SkillData != skillData)
                    continue;

                elementUI.Initialize(skillData, unitSkillComponent.GetSkill(skillType).Level);
                break;
            }
        }
    }
}
