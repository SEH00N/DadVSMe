using DadVSMe.Entities;
using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe.Players.FSM
{
    public class BumpAttackAction : FSMAction
    {
        [SerializeField] FSMState onGoingState = null;

        [Space(10f)]
        [SerializeField] AttackDataBase attackData = null;
        [SerializeField] float attackRange = 3f;

        private UnitFSMData unitFSMData = null;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            unitFSMData = brain.GetAIData<UnitFSMData>();
        }

        public override void EnterState()
        {
            base.EnterState();
            
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

            brain.ChangeState(onGoingState);
        }
    }
}
