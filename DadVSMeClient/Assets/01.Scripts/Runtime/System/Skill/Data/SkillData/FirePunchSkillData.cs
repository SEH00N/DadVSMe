using H00N.Resources.Addressables;
using UnityEngine;

namespace DadVSMe
{
    [CreateAssetMenu(fileName = "FirePunchSkillData", menuName = "DadVSMe/SkillData/Data/FirePunchSkillData")]
    public class FirePunchSkillData : SkillData
    {
        public float levelUpIncreaseRate;
        public AddressableAsset<ParticleSystem> particlePrefab;
    
        public override UnitSkill CreateSkill()
        {
            return new FirePunchSkill(levelUpIncreaseRate, particlePrefab);
        }
    }
}
