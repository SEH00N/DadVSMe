using UnityEngine;

namespace DadVSMe
{
    public abstract class Skill
    {
        protected GameObject owner;
        protected int level;

        public Skill()
        {
            owner = null;
            level = 1;
        }

        public virtual void OnRegist(GameObject owner)
        {
            this.owner = owner;
        }
        public abstract void OnUnregist();
        public abstract void OnExecute();

        public virtual void LevelUp()
        {
            level++;
        }
    }
}