using H00N.Resources.Addressables;
using UnityEngine;

namespace DadVSMe
{
    [CreateAssetMenu(fileName = "AttackBlastSkillData", menuName = "DadVSMe/SkillData/Data/AttackBlastSkillData")]
    public class AttackBlastSkillData : SkillData<AttackBlastSkill, AttackBlastSkillData.Option>
    {
        [System.Serializable]
        public class Option : SkillOption
        {
            public float attackBlastLifeTime;
            public int additiveDamage;
        }

        public AddressableAsset<AttackBlast> prefab;
    }
}
