using DadVSMe.Inputs;
using UnityEngine;
using H00N.AI.FSM;

namespace DadVSMe.Players.FSM
{
    public class PlayerMoveAction : FSMAction
    {
        private PlayerFSMData fsmData = null;
        private EntityMovement entityMovement = null;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            entityMovement = brain.GetComponent<EntityMovement>();
            fsmData = brain.GetAIData<PlayerFSMData>();
        }

        public override void EnterState()
        {
            base.EnterState();
            entityMovement.SetMovementVelocity(Vector2.zero);
        }

        public override void UpdateState()
        {
            base.UpdateState();

            PlayerInputReader inputReader = InputManager.GetInput<PlayerInputReader>();
            Vector2 movementInput = inputReader.MovementInput;
            Vector2 velocity = movementInput * fsmData.moveSpeed;

            if(inputReader.IsDashed)
                velocity.x = Mathf.Sign(velocity.x) * fsmData.dashSpeed;

            entityMovement.SetMovementVelocity(velocity);
        }

        public override void ExitState()
        {
            base.ExitState();
            entityMovement.SetMovementVelocity(Vector2.zero);
        }
    }
}