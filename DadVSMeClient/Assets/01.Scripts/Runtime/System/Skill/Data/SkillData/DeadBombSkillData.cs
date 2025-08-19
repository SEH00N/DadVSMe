using DadVSMe.Entities;
using H00N.Resources.Addressables;
using UnityEngine;

namespace DadVSMe
{
    [CreateAssetMenu(fileName = "DeadBombSkillData", menuName = "DadVSMe/SkillData/Data/DeadBombSkillData")]
    public class DeadBombSkillData : SkillData
    {
        public AttackDataBase attackData;
        public AddressableAsset<PoolableEffect> bombEffect;
        public AddressableAsset<AudioClip> bombSound;
        public float attackRadius;
        public float levelUpIncreaseRate;
        
        public override UnitSkill CreateSkill()
        {
            return new DeadBombSkill(attackData, bombEffect, bombSound, attackRadius, levelUpIncreaseRate);
        }
    }
}
