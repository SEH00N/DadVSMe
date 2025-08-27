using Cysharp.Threading.Tasks;
using H00N.Resources.Pools;
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
                elementUI.Initialize(skillData, unitSkill == null ? 0 : unitSkill.Level, this, i).Forget();
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

        public async UniTask DespawnUIAnimation()
        {
            SkillCardElementUI selected = default;
            
            for (int i = 0; i < elementUIList.Count; ++i)
            {
                var elementUI = elementUIList[i];

                if(elementUI.IsSelected == true)
                {
                    selected = elementUI;
                    continue;
                }

                await elementUI.PlayReleasAnimation();
            }

            await selected.PlayReleasAnimation();

            PoolManager.Despawn(this);
        }
    }
}