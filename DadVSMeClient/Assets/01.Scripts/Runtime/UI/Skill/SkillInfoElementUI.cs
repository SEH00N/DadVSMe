using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DadVSMe.UI.Skills
{
    public class SkillInfoElementUI : MonoBehaviourUI
    {
        [SerializeField] Color activeColor = Color.white;
        [SerializeField] Color inactiveColor = Color.white;
        [SerializeField] Color activeLevelColor = Color.white;
        [SerializeField] Color inactiveLevelColor = Color.white;

        [SerializeField] Image backgroundImage = null;
        [SerializeField] Image skillIconImage = null;
        [SerializeField] List<Image> levelObjectList = null;

        private SkillDataBase skillData;
        public SkillDataBase SkillData => skillData;

        public void Initialize(SkillDataBase skillData, int currentLevel)
        {
            this.skillData = skillData;

            bool isEmptySlot = skillData == null || currentLevel == 0;

            for(int i = 0; i < levelObjectList.Count; i++)
            {
                Image levelImage = levelObjectList[i];
                levelImage.color = i < currentLevel ? activeLevelColor : inactiveLevelColor;
                levelImage.gameObject.SetActive(isEmptySlot == false);
            }

            if(isEmptySlot == false)
                new SetSprite(skillIconImage, skillData.skillIcon);

            skillIconImage.gameObject.SetActive(isEmptySlot == false);
            backgroundImage.color = isEmptySlot ? inactiveColor : activeColor;
        }
    }
}