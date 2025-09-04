using DadVSMe.Entities;
using DadVSMe.Enemies.FSM;
using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe.Enemies
{
    public class ATVPatrolAction : FSMAction
    {
        private const float BOUNDARY_CHECK_DISTANCE = 3.5f;
        private const float PLAYER_CHECK_DISTANCE = 10f;

        private const int MAX_COUNTER = 10;

        private EnemyFSMData enemyFSMData = null;
        private UnitMovement unitMovement = null;
        private UnitStatData unitStatData = null;
        private UnitFSMData unitFSMData = null;
        
        private Vector2 patrolDirection = Vector2.zero;

        private int counter = 0;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            enemyFSMData = brain.GetAIData<EnemyFSMData>();
            unitMovement = brain.GetComponent<UnitMovement>();
            unitStatData = brain.GetAIData<UnitStatData>();
            unitFSMData = brain.GetAIData<UnitFSMData>();
        }

        public override void EnterState()
        {
            base.EnterState();
            unitMovement.SetActive(true);

            counter++;
            if(counter > MAX_COUNTER)
            {
                counter = 0;

                Vector2 targetPosition = enemyFSMData.Player.transform.position;
                Vector2 currentPosition = brain.transform.position;
                
                Vector3 direction = targetPosition - currentPosition;
                patrolDirection = direction.normalized;
            }
            else
            {
                patrolDirection = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)) * Vector2.right;
            }
        }

        public override void UpdateState()
        {
            base.UpdateState();

            // Vector2 targetPosition = enemyFSMData.Player.transform.position;
            // Vector2 currentPosition = brain.transform.position;
            // Vector2 targetDirection = targetPosition - currentPosition;
            // if(Mathf.Sign(targetDirection.x) != Mathf.Sign(patrolDirection.x) && targetDirection.sqrMagnitude >= PLAYER_CHECK_DISTANCE * PLAYER_CHECK_DISTANCE)
            // {
            //     brain.SetAsDefaultState();
            //     return;
            // }

            Vector2 movementDireciton = patrolDirection * unitStatData[EUnitStat.MoveSpeed].FinalValue;
            Vector2 movement = movementDireciton * (Time.deltaTime * BOUNDARY_CHECK_DISTANCE);

            Collider2D unitCollider = unitFSMData.unit.UnitCollider;
            if(Physics2D.OverlapBox((Vector2)unitCollider.bounds.center + movement, unitCollider.bounds.size, unitFSMData.unit.transform.eulerAngles.z, GameDefine.BOUNDARY_LAYER_MASK))
            {
                brain.SetAsDefaultState();
                return;
            }

            unitMovement.SetMovementVelocity(movementDireciton);
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
