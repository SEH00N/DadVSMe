using DadVSMe.Entities;
using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe.Enemies.FSM
{
    public class EnemyChaseAction : FSMAction
    {
        private const float UPDATE_INTERVAL = 0.1f;

        [SerializeField] float stopDistance = 1f;

        private EnemyFSMData enemyFSMData = null;
        private NPCMovement npcMovement = null;

        private float updateTimer = 0f;

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
        }

        public override void UpdateState()
        {
            base.UpdateState();

            Vector2 direction = (Vector2)(enemyFSMData.player.transform.position - brain.transform.position);
            if(direction.sqrMagnitude <= stopDistance * stopDistance)
                return;

            updateTimer += Time.deltaTime;
            if(updateTimer < UPDATE_INTERVAL)
                return;

            updateTimer = 0f;
            npcMovement.SetDestination(enemyFSMData.player.transform.position);
        }

        public override void ExitState()
        {
            base.ExitState();
            npcMovement.SetActive(false);
        }
    }
}
