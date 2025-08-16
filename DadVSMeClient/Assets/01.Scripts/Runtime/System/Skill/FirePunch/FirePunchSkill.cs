using DadVSMe.Entities;
using UnityEngine;

namespace DadVSMe
{
    public class FirePunchSkill : UnitSkill
    {
        public override void OnRegist(UnitSkillComponent ownerComponent)
        {
            base.OnRegist(ownerComponent);

            Execute();
        }

        public override void Execute()
        {
            ownerComponent.GetComponent<Unit>().SetAttackAttribute(EAttackAttribute.Fire);
        }

        public override void OnUnregist()
        {
            base.OnUnregist();

            ownerComponent.GetComponent<Unit>().SetAttackAttribute(EAttackAttribute.Normal);
        }
    }
}