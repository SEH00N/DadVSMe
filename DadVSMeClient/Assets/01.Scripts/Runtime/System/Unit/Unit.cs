using UnityEngine;
using H00N.AI.FSM;

namespace DadVSMe.Entities
{
    public class Unit : Entity
    {
        [Header("Unit")]
        [SerializeField] protected UnitData unitData = null;
        [SerializeField] protected UnitMovement unitMovement = null;
        [SerializeField] protected FSMBrain fsmBrain = null;
        [SerializeField] protected UnitHealth unitHealth = null;
        [SerializeField] protected UnitAttackEventListener unitAttackEventListener = null;
        [SerializeField] protected UnitSkillComponent unitSkillComponent;

        public UnitHealth UnitHealth => unitHealth; // uniy health is used frequently. allow external access for performance. 

        private UnitFSMData unitFSMData = null;

        public override void Initialize()
        {
            base.Initialize();
            fsmBrain.Initialize();
            fsmBrain.SetAsDefaultState();
            unitHealth.Initialize(unitData.maxHP);
            unitAttackEventListener.Initialize();
            unitSkillComponent.Initialize();

            unitFSMData = fsmBrain.GetAIData<UnitFSMData>();
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();

            if(unitMovement == null)
                return;

            if(unitMovement.IsActive == false)
                return;

            if(unitMovement.MovementVelocity.x == 0)
                return;

            int forwardDirection = unitMovement.MovementVelocity.x > 0 ? 1 : -1;
            entityAnimator.SetRotation(forwardDirection > 0);
            unitFSMData.forwardDirection = forwardDirection;
        }
    }
}