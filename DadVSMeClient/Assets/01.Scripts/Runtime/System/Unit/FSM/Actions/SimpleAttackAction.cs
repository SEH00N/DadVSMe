using UnityEngine;

namespace DadVSMe.Entities.FSM
{
    public class SimpleAttackAction : AttackActionBase
    {
        [SerializeField] AttackDataBase attackData = null;
        [SerializeField] UnitStateChecker unitStateChecker = null;

        protected override IAttackFeedbackDataContainer FeedbackDataContainer => attackData;

        protected override void OnAttack(EntityAnimationEventData eventData)
        {
            unitStateChecker.Check(unitFSMData.unit, unitFSMData.enemies, enemy => AttackToTarget(enemy, attackData));
        }
    }
}