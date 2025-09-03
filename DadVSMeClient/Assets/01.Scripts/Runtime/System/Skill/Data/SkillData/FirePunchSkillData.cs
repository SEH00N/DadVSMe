using DadVSMe.Entities;
using H00N.Resources.Addressables;
using UnityEngine;

namespace DadVSMe
{
    [CreateAssetMenu(fileName = "FirePunchSkillData", menuName = "DadVSMe/SkillData/Data/FirePunchSkillData")]
    public class FirePunchSkillData : SkillData<FirePunchSkill, FirePunchSkillData.Option>
    {
        [System.Serializable]
        public class Option : SkillOption
        {
            public float burnTime;
            public float attackDelay;
        }

        public AddressableAsset<ParticleSystem> particlePrefab;
        public AddressableAsset<Fire> firePrefab;
        public AttackDataBase attackData;
    }
}