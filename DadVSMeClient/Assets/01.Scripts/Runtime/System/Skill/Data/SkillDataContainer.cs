using ShibaInspector.Collections;
using UnityEngine;

namespace DadVSMe
{
    [CreateAssetMenu(fileName = "SkillDataContainer", menuName = "DadVSMe/SkillData/SkillContainer")]
    public class SkillDataContainer : ScriptableObject
    {
        [SerializeField] private SerializableDictionary<SkillType, SkillData> datas;

        public UnitSkill CreateSkill(SkillType skillType)
        {
            if (datas.ContainsKey(skillType))
            {
                return datas[skillType].CreateSkill();
            }

            return null;
        }

        public SkillData GetSkillData(SkillType skillType)
        {
            return datas[skillType];
        }
    }
}