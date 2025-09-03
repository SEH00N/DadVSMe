using DadVSMe.Entities;
using H00N.Resources.Addressables;
using UnityEngine;

namespace DadVSMe
{
    [CreateAssetMenu(fileName = "StatikkShivSkillData", menuName = "DadVSMe/SkillData/Data/StatikkShivSkillData")]
    public class StatikkShivSkillData : SkillData<StatikkShivSkill, StatikkShivSkillData.Option>
    {
        [System.Serializable]
        public class Option : SkillOption
        {
            [Range(0f, 1f)] public float executeChance;
            public int damage;
        }

        public AddressableAsset<StatikkShivLighting> prefab;
        public AttackDataBase attackData;
        public float checkRadius;
    }
}
