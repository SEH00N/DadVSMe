using DadVSMe.Entities.FSM;
using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe.Enemies.FSM
{
    public class EnemyButtAttackAction : DashAttackAction
    {
        private EnemyFSMData enemyFSMData = null;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            enemyFSMData = brain.GetAIData<EnemyFSMData>();
        }

        public override void EnterState()
        {
            int forwardDirection = enemyFSMData.Player.transform.position.x > brain.transform.position.x ? 1 : -1;
            unitFSMData.forwardDirection = forwardDirection;

            float currentLossyScaleX = brain.transform.lossyScale.x;
            brain.transform.localScale = new Vector3(brain.transform.localScale.x * (unitFSMData.forwardDirection / currentLossyScaleX), 1, 1);

            base.EnterState();
        }
    }
}