using H00N.Resources.Addressables;
using UnityEngine;
using UnityEngine.Localization;

namespace DadVSMe
{
    public abstract class SkillData : ScriptableObject
    {
        public SkillType skillType;
        public LocalizedString skillName;
        public LocalizedString skillDesc;
        public LocalizedString skillLevelUpDesc;
        public AddressableAsset<Sprite> skillIcon;

        public abstract UnitSkill CreateSkill();
    }
}
