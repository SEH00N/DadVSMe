using DadVSMe.Inputs;
using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe.Entities.FSM
{
    public class DashAttackAction : AttackActionBase
    {
        [Space(10f)]
        [SerializeField] float speed = 10f;
        [SerializeField] bool moveStopping = false;
        
        [Space(10f)]
        [SerializeField] UnitStateChecker unitStateChecker = null;

        private UnitMovement unitMovement = null;

        private bool isAttacking = false;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            unitMovement = brain.GetComponent<UnitMovement>();
        }

        public override void EnterState()
        {
            base.EnterState();

            unitMovement.SetActive(true);
            unitMovement.SetMovementVelocity(unitFSMData.forwardDirection * speed * Vector2.right);

            isAttacking = false;
        }

        public override void UpdateState()
        {
            base.UpdateState();

            if(isAttacking == false)
                return;

            if(unitMovement.MovementVelocity.x == 0)
                return;

            unitStateChecker.Check(unitFSMData.unit, unitFSMData.enemies, enemy => AttackToTarget(enemy, attackData));
        }

        public override void ExitState()
        {
            base.ExitState();
            unitMovement.SetActive(false);

            isAttacking = false;
        }

        protected override void OnAttack(EntityAnimationEventData eventData)
        {
            isAttacking = !isAttacking;
            if(isAttacking == false && moveStopping)
                unitMovement.SetMovementVelocity(Vector2.zero);
        }
    }
}
