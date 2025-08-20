using H00N.Resources.Addressables;
using UnityEngine;

namespace DadVSMe
{
    [CreateAssetMenu(fileName = "GuidedOrbSkillData", menuName = "DadVSMe/SkillData/Data/GuidedOrbSkillData")]
    public class GuidedOrbSkillData : SkillData
    {
        public AddressableAsset<GuidedOrb> prefab = null;
        public AddressableAsset<AudioClip> sound;
        public float coolTime;
        public int levelUpIncreaseRate;

        public override UnitSkill CreateSkill()
        {
            return new GuidedOrbSkill(prefab, coolTime, levelUpIncreaseRate, sound);
        }
    }
}
