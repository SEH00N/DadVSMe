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
            public int stackCount;
            public float cooltime;
            public int damage;
        }

        public AddressableAsset<AttackBlast> prefab;
        public float attackBlastLifeTime;
    }
}
