using DadVSMe.Entities;
using H00N.Resources.Addressables;
using UnityEngine;

namespace DadVSMe
{
    [CreateAssetMenu(fileName = "GuidedOrbSkillData", menuName = "DadVSMe/SkillData/Data/GuidedOrbSkillData")]
    public class GuidedOrbSkillData : SkillData<GuidedOrbSkill, GuidedOrbSkillData.Option>
    {
        [System.Serializable]
        public class Option : SkillOption
        {
            public float coolTime;
            public int spawnCount;
            public int damage;
        }

        public AddressableAsset<GuidedOrb> prefab = null;
        public AttackDataBase attackData;
    }
}
