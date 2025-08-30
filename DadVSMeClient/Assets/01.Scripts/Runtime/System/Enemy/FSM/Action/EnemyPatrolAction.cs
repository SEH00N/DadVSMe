using DadVSMe.Entities;
using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe.Enemies.FSM
{
    public class EnemyPatrolAction : FSMAction
    {
        private const float BOUNDARY_CHECK_DISTANCE = 0.5f;

        private EnemyFSMData enemyFSMData = null;
        private UnitMovement unitMovement = null;
        private UnitStatData unitStatData = null;

        private Vector2 patrolPoint = Vector2.zero;

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
            
            Vector3 direction = currentPosition - targetPosition;
            float distance = direction.magnitude;

            Vector2 validDirection = direction.normalized * Mathf.Max(enemyFSMData.patrolMinRange - distance, 0);

            Vector2 randomDirection = Random.insideUnitCircle;
            randomDirection -= Vector3.Dot(randomDirection, validDirection.normalized) * validDirection.normalized;
            randomDirection.Normalize();

            float randomRadius = Mathf.Sqrt(Random.Range(enemyFSMData.patrolMinRange * enemyFSMData.patrolMinRange, enemyFSMData.patrolMaxRange * enemyFSMData.patrolMaxRange));
            patrolPoint = targetPosition + (randomDirection * randomRadius);
        }

        public override void UpdateState()
        {
            base.UpdateState();

            Vector2 direction = patrolPoint - (Vector2)brain.transform.position;
            if(Physics2D.Raycast(brain.transform.position, direction.normalized, BOUNDARY_CHECK_DISTANCE, GameDefine.BOUNDARY_LAYER_MASK))
            {
                brain.SetAsDefaultState();
                return;
            }

            unitMovement.SetMovementVelocity(direction.normalized * unitStatData[EUnitStat.MoveSpeed].FinalValue);

            if(direction.sqrMagnitude >= 0.01f)
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
