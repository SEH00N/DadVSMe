using H00N.Resources.Addressables;
using UnityEngine;

namespace DadVSMe
{
    [CreateAssetMenu(fileName = "GuidedOrbSkillData", menuName = "DadVSMe/SkillData/Data/GuidedOrbSkillData")]
    public class GuidedOrbSkillData : SkillData
    {
        public AddressableAsset<GuidedOrb> prefab = null;
        public float coolTime;

        public override UnitSkill CreateSkill()
        {
            return new GuidedOrbSkill(prefab, coolTime);
        }
    }
}
