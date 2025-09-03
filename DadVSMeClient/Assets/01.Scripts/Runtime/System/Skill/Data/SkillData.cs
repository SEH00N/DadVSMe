using H00N.Resources.Addressables;
using UnityEngine;
using UnityEngine.Localization;

namespace DadVSMe
{
    public abstract class SkillOption 
    { 
        public LocalizedString skillDesc;
    }

    public abstract class SkillDataBase : ScriptableObject
    {
        public SkillType skillType;
        public LocalizedString skillName;
        public AddressableAsset<Sprite> skillIcon;

        public abstract UnitSkillBase CreateSkill();

        public SkillOption GetOption(int level) => GetOption<SkillOption>(level);
        public abstract T GetOption<T>(int level) where T : SkillOption;
    }
    
    public abstract class SkillData<TSkill, TOption> : SkillDataBase where TSkill : UnitSkillBase, new() where TOption : SkillOption
    {
        [SerializeField] TOption[] optionsPerLevel = new TOption[GameDefine.MAX_SKILL_LEVEL];

        public sealed override T GetOption<T>(int level) => optionsPerLevel[level - 1] as T;
        public sealed override UnitSkillBase CreateSkill() => new TSkill();
    }
}
