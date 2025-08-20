using UnityEngine;

namespace DadVSMe
{
    public class StatUpSkill : UnitSkill
    {
        public override void OnRegist(UnitSkillComponent ownerComponent)
        {
            base.OnRegist(ownerComponent);

            Execute();
        }

        public override void LevelUp()
        {
            base.LevelUp();

            Execute();
        }

        public override void Execute()
        {
            
        }
    }
}
