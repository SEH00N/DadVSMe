using DadVSMe.Entities;
using H00N.Resources.Addressables;
using UnityEngine;

namespace DadVSMe
{
    [CreateAssetMenu(fileName = "DeadBombSkillData", menuName = "DadVSMe/SkillData/Data/DeadBombSkillData")]
    public class DeadBombSkillData : SkillData
    {
        public AttackDataBase attackData;
        public AddressableAsset<Bomb> bombRef;
        public float attackRadius;
        public float levelUpIncreaseRate;
        
        public override UnitSkill CreateSkill()
        {
            return new DeadBombSkill(attackData, bombRef, attackRadius, levelUpIncreaseRate);
        }
    }
}
