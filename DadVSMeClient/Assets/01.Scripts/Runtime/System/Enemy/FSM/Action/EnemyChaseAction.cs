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
        private UnitMovement unitMovement = null;
        private UnitStatData unitStatData = null;

        private float updateTimer = 0f;

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
        }

        public override void UpdateState()
        {
            base.UpdateState();

            updateTimer += Time.deltaTime;
            if(updateTimer < UPDATE_INTERVAL)
                return;

            updateTimer = 0f;

            Vector2 direction = (Vector2)(enemyFSMData.Player.transform.position - brain.transform.position);
            Vector3 destination = enemyFSMData.Player.transform.position + new Vector3(xPadding * -Mathf.Sign(direction.x), 0f, 0f);
            
            unitMovement.SetMovementVelocity(destination.normalized * unitStatData[EUnitStat.MoveSpeed].FinalValue);
        }

        public override void ExitState()
        {
            base.ExitState();
            unitMovement.SetActive(false);
        }
    }
}
