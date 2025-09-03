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

        [SerializeField] Player player = null;
        [SerializeField] UnitSkillComponent playerSkillComponent = null;
        [SerializeField] AddressableAsset<SkillSelectPopupUI> skillSelectPopupUIPrefab = null;
        [SerializeField] AddressableAsset<AudioClip> levelUpSound = null;

        private void Awake()
        {
            skillSelectPopupUIPrefab.InitializeAsync().Forget();
            levelUpSound.InitializeAsync().Forget();
        }

        public async void OnLevelUp(int _)
        {
            if(GameInstance.GameCycle.IsPaused)
                await UniTask.WaitUntil(() => GameInstance.GameCycle.IsPaused == false);

            new PlaySound(levelUpSound);
            
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
                SkillType skillType;
                if(playerSkillComponent.SkillContainer.Count >= GameDefine.SKILL_INVENTORY_COUNT)
                {
                    skillType = SkillType.HealPack;
                }
                else
                {
                    skillType = EnumHelper.GetRandomValue<SkillType>();
                    if(skillType == SkillType.HealPack)
                        continue;
                }

                if(skillType != SkillType.HealPack)
                {
                    UnitSkill unitSkill = playerSkillComponent.GetSkill(skillType);
                    if(unitSkill != null && unitSkill.Level >= GameDefine.MAX_SKILL_LEVEL)
                        continue;

                    if(availableSkillTypes.Contains(skillType))
                        continue;
                }

                availableSkillTypes.Add(skillType);
            }

            return availableSkillTypes;
        }

        public async void OnSelectSkill(SkillSelectPopupUI popupUI, SkillData skillData)
        {
            if(skillData.skillType == SkillType.HealPack)
                player.UnitHealth.Heal((skillData as HealPackSkillData).healAmount);
            else
                playerSkillComponent.RegistSkill(skillData.skillType);

            await popupUI.DespawnUIAnimation();

            TimeManager.SetTimeScale(1, true);
        }
    }
}