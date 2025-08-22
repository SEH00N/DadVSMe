using System.Collections.Generic;
using UnityEngine;

namespace DadVSMe.UI.Skills
{
    public class SkillSelectPopupUI : PoolableBehaviourUI<SkillSelectPopupUI.ICallback>, SkillCardElementUI.ICallback
    {
        public interface ICallback : IUICallback
        {
            void OnSelectSkill(SkillSelectPopupUI popupUI, SkillData skillData);
        }

        [SerializeField] List<SkillCardElementUI> elementUIList = new List<SkillCardElementUI>();

        public void Initialize(UnitSkillComponent skillComponent, List<SkillType> skillList, ICallback callback)
        {
            base.Initialize(callback);

            for(int i = 0; i < elementUIList.Count; ++i)
            {
                SkillCardElementUI elementUI = elementUIList[i];
                if(skillList.Count <= i)
                {
                    elementUI.gameObject.SetActive(false);
                    continue;
                }

                if(elementUI.gameObject.activeSelf == false)
                    elementUI.gameObject.SetActive(true);

                SkillType skillType = skillList[i];
                SkillData skillData = skillComponent.SkillDataContainer.GetSkillData(skillType);
                UnitSkill unitSkill = skillComponent.GetSkill(skillType);
                elementUI.Initialize(skillData, unitSkill == null ? 0 : unitSkill.Level, this);
            }
        }

        protected override void Release()
        {
            base.Release();
        }

        public void OnSelectCard(SkillData skillData)
        {
            callback.OnSelectSkill(this, skillData);
        }
    }
}