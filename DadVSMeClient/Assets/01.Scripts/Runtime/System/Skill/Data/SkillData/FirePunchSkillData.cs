using DadVSMe.Entities;
using H00N.Resources.Addressables;
using UnityEngine;

namespace DadVSMe
{
    [CreateAssetMenu(fileName = "FirePunchSkillData", menuName = "DadVSMe/SkillData/Data/FirePunchSkillData")]
    public class FirePunchSkillData : SkillData
    {
        public float levelUpIncreaseRate;
        public AddressableAsset<ParticleSystem> particlePrefab;
        public AddressableAsset<Fire> firePrefab;
        public float burnTime;
        public float attackDelay;
        public AttackDataBase attackData;
    
        public override UnitSkill CreateSkill()
        {
            return new FirePunchSkill(attackData, levelUpIncreaseRate, firePrefab, burnTime, attackDelay);
        }
    }
}