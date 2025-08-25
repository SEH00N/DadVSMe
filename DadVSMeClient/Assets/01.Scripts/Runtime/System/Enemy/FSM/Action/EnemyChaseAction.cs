using DadVSMe.Entities;
using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe.Enemies.FSM
{
    public class EnemyChaseAction : FSMAction
    {
        private const float UPDATE_INTERVAL = 0.1f;

        [SerializeField] float xPadding = 1f;

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

            updateTimer += Time.deltaTime;
            if(updateTimer < UPDATE_INTERVAL)
                return;

            updateTimer = 0f;

            Vector2 direction = (Vector2)(enemyFSMData.Player.transform.position - brain.transform.position);
            npcMovement.SetDestination(enemyFSMData.Player.transform.position + new Vector3(xPadding * -Mathf.Sign(direction.x), 0f, 0f));
        }

        public override void ExitState()
        {
            base.ExitState();
            npcMovement.SetActive(false);
        }
    }
}
