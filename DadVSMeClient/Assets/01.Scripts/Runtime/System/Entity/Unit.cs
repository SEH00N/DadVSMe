using UnityEngine;
using H00N.AI.FSM;

namespace DadVSMe.Entities
{
    public class Unit : Entity
    {
        [SerializeField] protected UnitData unitData = null;
        [SerializeField] protected UnitMovement unitMovement = null;
        [SerializeField] protected FSMBrain fsmBrain = null;
        [SerializeField] protected UnitHealth unitHealth = null;
        [SerializeField] protected UnitAttackEventListener unitAttackEventListener = null;

        public UnitHealth UnitHealth => unitHealth; // uniy health is used frequently. allow external access for performance. 

        public override void Initialize()
        {
            base.Initialize();
            fsmBrain.Initialize();
            fsmBrain.SetAsDefaultState();
            unitHealth.Initialize(unitData.maxHP);
            unitAttackEventListener.Initialize();
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