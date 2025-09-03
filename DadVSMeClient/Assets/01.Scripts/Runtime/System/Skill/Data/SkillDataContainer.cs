using ShibaInspector.Collections;
using UnityEngine;

namespace DadVSMe
{
    [CreateAssetMenu(fileName = "SkillDataContainer", menuName = "DadVSMe/SkillData/SkillContainer")]
    public class SkillDataContainer : ScriptableObject
    {
        [SerializeField] private SerializableDictionary<SkillType, SkillDataBase> datas;

        public UnitSkillBase CreateSkill(SkillType skillType)
        {
            if (datas.ContainsKey(skillType))
            {
                return datas[skillType].CreateSkill();
            }

            return null;
        }

        public SkillDataBase GetSkillData(SkillType skillType)
        {
            return datas[skillType];
        }
    }
}