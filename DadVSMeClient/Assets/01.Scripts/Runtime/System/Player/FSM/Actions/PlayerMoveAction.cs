using DadVSMe.Inputs;
using UnityEngine;
using H00N.AI.FSM;
using DadVSMe.Entities;

namespace DadVSMe.Players.FSM
{
    public class PlayerMoveAction : FSMAction
    {
        [SerializeField] bool isDashing = false;

        private UnitStat moveSpeedStat;
        private UnitStat dashSpeedStat;
        private UnitMovement unitMovement = null;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            unitMovement = brain.GetComponent<UnitMovement>();
            moveSpeedStat = brain.GetAIData<UnitStatData>()[EUnitStat.MoveSpeed];
            dashSpeedStat = brain.GetAIData<UnitStatData>()[EUnitStat.DashSpeed];
        }

        public override void EnterState()
        {
            base.EnterState();
            unitMovement.SetActive(true);
        }

        public override void UpdateState()
        {
            base.UpdateState();

            PlayerInputReader inputReader = InputManager.GetInput<PlayerInputReader>();
            Vector2 movementInput = inputReader.MovementInput;
            
            Vector2 velocity = movementInput * moveSpeedStat.FinalValue;
            if (velocity.x != 0 && isDashing)
                velocity.x = Mathf.Sign(velocity.x) * dashSpeedStat.FinalValue;

            unitMovement.SetMovementVelocity(velocity);
        }

        public override void ExitState()
        {
            base.ExitState();
            unitMovement.SetActive(false);
        }
    }
}