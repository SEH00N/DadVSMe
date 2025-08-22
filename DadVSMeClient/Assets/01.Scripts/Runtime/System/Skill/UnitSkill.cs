using UnityEngine;

namespace DadVSMe
{
    public abstract class UnitSkill
    {
        protected UnitSkillComponent ownerComponent;
        protected int level;
        public int Level => level;

        public UnitSkill()
        {
            ownerComponent = null;
            level = 1;
        }

        public virtual void OnRegist(UnitSkillComponent ownerComponent)
        {
            this.ownerComponent = ownerComponent;
        }
        public virtual void OnUnregist() { }
        public abstract void Execute();

        public virtual void LevelUp()
        {
            level++;
        }
    }
}