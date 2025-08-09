using UnityEngine;
using H00N.AI.FSM;

namespace DadVSMe.Entities
{
    public class Unit : Entity
    {
        [SerializeField] protected UnitMovement unitMovement = null;
        [SerializeField] protected FSMBrain fsmBrain = null;

        public override void Initialize()
        {
            base.Initialize();
            fsmBrain.Initialize();
            fsmBrain.SetAsDefaultState();
        }

        private void LateUpdate()
        {
            if(unitMovement == null)
                return;

            if(unitMovement.IsActive == false)
                return;

            entityAnimator.SetRotation(unitMovement.MovementVelocity.x > 0);
        }        
    }
}