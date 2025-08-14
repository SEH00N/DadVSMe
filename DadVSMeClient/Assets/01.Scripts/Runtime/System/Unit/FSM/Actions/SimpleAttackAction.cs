using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe.Entities.FSM
{
    public class SimpleAttackAction : AttackActionBase
    {
        [SerializeField] float attackRange = 3f;
        private UnitFSMData unitFSMData = null;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            unitFSMData = brain.GetAIData<UnitFSMData>();
        }

        protected override void OnAttack(EntityAnimationEventData eventData)
        {
            int forwardDirection = unitFSMData.forwardDirection;
            unitFSMData.enemies.ForEach(enemy => {
                float targetDirection = enemy.transform.position.x - transform.position.x;
                if(Mathf.Sign(targetDirection) != Mathf.Sign(forwardDirection))
                    return;

                if(Mathf.Abs(targetDirection) > attackRange)
                    return;

                AttackToTarget(enemy);
            });
        }
    }
}