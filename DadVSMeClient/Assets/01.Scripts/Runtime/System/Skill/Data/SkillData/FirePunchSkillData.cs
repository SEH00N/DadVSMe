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
            public float attackCount;
            public int damage;
        }

        public AddressableAsset<ParticleSystem> particlePrefab;
        public AddressableAsset<Fire> firePrefab;
        public AttackDataBase attackData;
        public float attackDelay;
    }
}