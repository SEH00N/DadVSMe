using DadVSMe.Entities;
using DadVSMe.Enemies.FSM;
using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe.Enemies
{
    public class ATVPatrolAction : FSMAction
    {
        private const float BOUNDARY_CHECK_DISTANCE = 3f;
        private const float PLAYER_CHECK_DISTANCE = 10f;

        private EnemyFSMData enemyFSMData = null;
        private UnitMovement unitMovement = null;
        private UnitStatData unitStatData = null;

        private Vector2 patrolDirection = Vector2.zero;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            enemyFSMData = brain.GetAIData<EnemyFSMData>();
            unitMovement = brain.GetComponent<UnitMovement>();
            unitStatData = brain.GetAIData<UnitStatData>();
        }

        public override void EnterState()
        {
            base.EnterState();
            unitMovement.SetActive(true);

            Vector2 targetPosition = enemyFSMData.Player.transform.position;
            Vector2 currentPosition = brain.transform.position;
            
            Vector3 direction = targetPosition - currentPosition;
            patrolDirection = direction.normalized;
        }

        public override void UpdateState()
        {
            base.UpdateState();

            Vector2 targetPosition = enemyFSMData.Player.transform.position;
            Vector2 currentPosition = brain.transform.position;
            Vector2 targetDirection = targetPosition - currentPosition;
            if(Mathf.Sign(targetDirection.x) != Mathf.Sign(patrolDirection.x) && targetDirection.sqrMagnitude >= PLAYER_CHECK_DISTANCE * PLAYER_CHECK_DISTANCE)
            {
                brain.SetAsDefaultState();
                return;
            }

            if(Physics2D.Raycast(brain.transform.position, patrolDirection, BOUNDARY_CHECK_DISTANCE, GameDefine.BOUNDARY_LAYER_MASK))
            {
                brain.SetAsDefaultState();
                return;
            }

            unitMovement.SetMovementVelocity(patrolDirection * unitStatData[EUnitStat.MoveSpeed].FinalValue);
            if(patrolDirection.sqrMagnitude >= 0.01f)
                return;

            brain.SetAsDefaultState();
        }

        public override void ExitState()
        {
            base.ExitState();
            unitMovement.SetActive(false);
        }
    }
}
