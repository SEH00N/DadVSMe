using UnityEngine;

namespace DadVSMe.Entities.FSM
{
    public class SimpleAttackAction : AttackActionBase
    {
        [SerializeField] UnitStateChecker unitStateChecker = null;

        protected override void OnAttack(EntityAnimationEventData eventData)
        {
            unitStateChecker.Check(unitFSMData.unit, unitFSMData.enemies, enemy => AttackToTarget(enemy, attackData));
        }
    }
}