using H00N.Resources.Addressables;
using UnityEngine;

namespace DadVSMe
{
    [CreateAssetMenu(fileName = "StatikkShivSkillData", menuName = "DadVSMe/SkillData/Data/StatikkShivSkillData")]
    public class StatikkShivSkillData : SkillData
    {
        public AddressableAsset<StatikkShivLighting> prefab;
        public AddressableAsset<AudioClip> sound;
        public float checkTime;
        public int targetAttackCount;
        public int maxAttackTargetNum;
        public float checkRadius;
        public int levelUpIncreaseRate;

        public override UnitSkill CreateSkill()
        {
            return new StatikkShivSkill(prefab, sound, checkTime, targetAttackCount, maxAttackTargetNum, checkRadius, levelUpIncreaseRate);
        }
    }
}
