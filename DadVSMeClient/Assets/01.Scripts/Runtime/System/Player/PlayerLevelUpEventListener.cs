using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DadVSMe.UI.Skills;
using H00N.Resources.Addressables;
using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe.Players
{
    public class PlayerLevelUpEventListener : MonoBehaviour, SkillSelectPopupUI.ICallback
    {
        private const int SKILL_COUNT = 3;

        [SerializeField] UnitSkillComponent playerSkillComponent;
        [SerializeField] AddressableAsset<SkillSelectPopupUI> skillSelectPopupUIPrefab;

        private void Awake()
        {
            skillSelectPopupUIPrefab.InitializeAsync().Forget();
        }

        [ContextMenu("Level up")]
        public void OnLevelUp() => OnLevelUp(0);

        public async void OnLevelUp(int _)
        {
            if(GameInstance.GameCycle.IsPaused)
                await UniTask.WaitUntil(() => GameInstance.GameCycle.IsPaused == false);

            SkillSelectPopupUI skillSelectPopupUI = PoolManager.Spawn<SkillSelectPopupUI>(skillSelectPopupUIPrefab, GameInstance.MainPopupFrame);
            skillSelectPopupUI.StretchRect();
            skillSelectPopupUI.Initialize(playerSkillComponent, GetAvailableSkillTypes(playerSkillComponent), this);

            TimeManager.SetTimeScale(0, true);
        }

        private static List<SkillType> GetAvailableSkillTypes(UnitSkillComponent playerSkillComponent)
        {
            List<SkillType> availableSkillTypes = new List<SkillType>(SKILL_COUNT);
            while(availableSkillTypes.Count < SKILL_COUNT)
            {
                SkillType skillType = EnumHelper.GetRandomValue<SkillType>();

                UnitSkill unitSkill = playerSkillComponent.GetSkill(skillType);
                if(unitSkill != null && unitSkill.Level >= GameDefine.MAX_SKILL_LEVEL)
                    continue;

                if(availableSkillTypes.Contains(skillType))
                    continue;

                availableSkillTypes.Add(skillType);
            }

            return availableSkillTypes;
        }

        public void OnSelectSkill(SkillSelectPopupUI popupUI, SkillData skillData)
        {
            playerSkillComponent.RegistSkill(skillData.skillType);
            TimeManager.SetTimeScale(1, true);
            PoolManager.Despawn(popupUI);
        }
    }
}