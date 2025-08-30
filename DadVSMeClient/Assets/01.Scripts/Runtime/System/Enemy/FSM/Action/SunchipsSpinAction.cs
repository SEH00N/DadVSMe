using DadVSMe.Enemies.FSM;
using DadVSMe.Entities;
using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe.Enemies.FSM
{
    public class SunchipsSpinAction : FSMAction
    {
        [SerializeField] float speed = 10f;

        private UnitMovement unitMovement = null;
        private EntityAnimator entityAnimator = null;
        private UnitFSMData unitFSMData = null;
        private SunchipsEnemyFSMData fsmData;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            unitMovement = brain.GetComponent<UnitMovement>();
            entityAnimator = brain.GetComponent<EntityAnimator>();
            unitFSMData = brain.GetAIData<UnitFSMData>();
            fsmData = brain.GetAIData<SunchipsEnemyFSMData>();
        }

        public override void EnterState()
        {
            base.EnterState();

            if (unitFSMData.enemies.Count <= 0)
                return;

            fsmData.frenzyTarget = unitFSMData.enemies[0] as Unit;

            unitMovement.SetActive(true);
        }

        public override void UpdateState()
        {
            base.UpdateState();

            Vector3 dir = (fsmData.frenzyTarget.transform.position - brain.transform.position).normalized;

            unitMovement.SetMovementVelocity(dir * speed);
        }

        public override void ExitState()
        {
            base.ExitState();
            unitMovement.SetActive(false);
            fsmData.currentFrenzyCooltime = fsmData.frenzyCoolTime;
        }

        public void ChangeMoveDirection()
        {

        }
    }
}
