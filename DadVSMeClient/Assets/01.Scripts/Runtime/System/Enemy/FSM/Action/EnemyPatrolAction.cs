using DadVSMe.Entities;
using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe.Enemies.FSM
{
    public class EnemyPatrolAction : FSMAction
    {
        [SerializeField] float patrolMinRange = 10f;
        [SerializeField] float patrolMaxRange = 30f;

        private EnemyFSMData enemyFSMData = null;
        private NPCMovement npcMovement = null;

        private Vector2 patrolPoint = Vector2.zero;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            enemyFSMData = brain.GetAIData<EnemyFSMData>();
            npcMovement = brain.GetComponent<NPCMovement>();
        }

        public override void EnterState()
        {
            base.EnterState();
            npcMovement.SetActive(true);

            Vector2 targetPosition = enemyFSMData.player.transform.position;
            Vector2 currentPosition = brain.transform.position;
            
            Vector3 direction = currentPosition - targetPosition;
            float distance = direction.magnitude;

            Vector2 validDirection = direction.normalized * Mathf.Max(patrolMinRange - distance, 0);

            Vector2 randomDirection = Random.insideUnitCircle;
            randomDirection -= Vector3.Dot(randomDirection, validDirection.normalized) * validDirection.normalized;
            randomDirection.Normalize();

            float randomRadius = Mathf.Sqrt(Random.Range(patrolMinRange * patrolMinRange, patrolMaxRange * patrolMaxRange));
            patrolPoint = npcMovement.GetValidDestination(targetPosition + (randomDirection * randomRadius));

            npcMovement.SetDestination(patrolPoint);
        }

        public override void UpdateState()
        {
            base.UpdateState();

            Vector2 direction = patrolPoint - (Vector2)brain.transform.position;
            if(direction.sqrMagnitude >= 0.01f * 0.01f)
                return;

            brain.SetAsDefaultState();
        }

        public override void ExitState()
        {
            base.ExitState();
            npcMovement.SetActive(false);
        }
    }
}
