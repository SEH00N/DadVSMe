using DadVSMe.Inputs;
using DadVSMe.Entities;
using DadVSMe.Entities.FSM;
using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe.Players.FSM
{
    public class DashSlamAction : AttackActionBase
    {
        [SerializeField] float speed = 10f;

        [Space(10f)]
        [SerializeField] AttackDataBase attackData = null;
        [SerializeField] float attackRange = 3f;

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

            PlayerInputReader inputReader = InputManager.GetInput<PlayerInputReader>();
            inputReader.ReleaseDash();

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

                if(Mathf.Abs(targetDirection) > attackRange)
                    return;

                if(enemy.FSMBrain.GetAIData<UnitFSMData>().isFloat)
                    return;

                if(enemy.FSMBrain.GetAIData<UnitFSMData>().isLie)
                    return;

                enemy.UnitHealth.Attack(unitFSMData.unit, attackData);
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
            if(isAttacking == false)
                unitMovement.SetMovementVelocity(Vector2.zero);
        }
    }
}
