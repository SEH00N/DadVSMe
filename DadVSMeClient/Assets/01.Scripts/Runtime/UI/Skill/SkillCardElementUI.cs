using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DadVSMe.UI.Skills
{
    public class SkillCardElementUI : PoolableBehaviourUI<SkillCardElementUI.ICallback>
    {
        public interface ICallback : IUICallback
        {
            void OnSelectCard(SkillData skillData);
        }

        private const float TRANSITION_TIME = 0.75f;
        private const string BLOCK_KEY = "SkillCardElementUI";

        [SerializeField] Image skillIcon = null;
        [SerializeField] TMP_Text nameText = null;
        [SerializeField] TMP_Text descText = null;
        [SerializeField] List<GameObject> levelObjectList = null;

        private SkillData skillData = null;

        public void Initialize(SkillData skillData, int currentLevel, ICallback callback)
        {
            base.Initialize(callback);
            this.skillData = skillData;

            new SetSprite(skillIcon, skillData.skillIcon);
            nameText.text = skillData.skillName.ToString();
            descText.text = skillData.skillDesc.ToString();

            for(int i = 0; i < levelObjectList.Count; i++)
                levelObjectList[i].SetActive(i < currentLevel);
        }

        public async void OnTouchThis()
        {
            for(int i = 0; i < levelObjectList.Count; i++)
            {
                if(levelObjectList[i].activeSelf)
                    continue;

                levelObjectList[i].SetActive(true);
                break;
            }

            InputBlock.Block(BLOCK_KEY);
            await UniTask.Delay(TimeSpan.FromSeconds(TRANSITION_TIME), true);
            InputBlock.Release(BLOCK_KEY);

            callback.OnSelectCard(skillData);
        }
    }
}