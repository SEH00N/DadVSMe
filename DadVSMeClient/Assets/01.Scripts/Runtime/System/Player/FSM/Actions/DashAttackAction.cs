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
        [SerializeField] AttackDataBase attackData = null;
        
        [Space(10f)]
        [SerializeField] bool checkHorizontalRange = true;
        [SerializeField] float attackHorizontalRange = 3f;
        [SerializeField] bool checkVerticalRange = true;
        [SerializeField] float attackVerticalRange = 1f;

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

            int forwardDirection = unitFSMData.forwardDirection;
            unitFSMData.enemies.ForEach(enemy => {
                float targetDirection = enemy.transform.position.x - transform.position.x;
                if(Mathf.Sign(targetDirection) != Mathf.Sign(forwardDirection))
                    return;

                if(checkHorizontalRange)
                {
                    if(Mathf.Abs(targetDirection) > attackHorizontalRange)
                        return;
                }

                if(checkVerticalRange)
                {
                    if(Mathf.Abs(enemy.transform.position.y - transform.position.y) > attackVerticalRange)
                        return;
                }

                if(enemy.FSMBrain.GetAIData<UnitFSMData>().isFloat)
                    return;

                if(enemy.FSMBrain.GetAIData<UnitFSMData>().isLie)
                    return;

                AttackToTarget(enemy, attackData);
            });
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
