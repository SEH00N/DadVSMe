using H00N.Resources.Addressables;
using UnityEngine;

namespace DadVSMe
{
    [CreateAssetMenu(fileName = "AttackBlastSkillData", menuName = "DadVSMe/SkillData/Data/AttackBlastSkillData")]
    public class AttackBlastSkillData : SkillData
    {
        public AddressableAsset<AttackBlast> prefab;
        public float attackBlastLifeTime;
        public float levelUpIncreaseRate;

        public override UnitSkill CreateSkill()
        {
            return new AttackBlastSkill(prefab, attackBlastLifeTime, levelUpIncreaseRate);
        }
    }
}
