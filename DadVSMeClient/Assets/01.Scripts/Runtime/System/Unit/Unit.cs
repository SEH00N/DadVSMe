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
        [SerializeField] protected Rigidbody2D unitRigidbody = null;

        public FSMBrain FSMBrain => fsmBrain;
        public UnitHealth UnitHealth => unitHealth; // uniy health is used frequently. allow external access for performance. 
        
        protected virtual RigidbodyType2D DefaultRigidbodyType => RigidbodyType2D.Kinematic;

        private UnitFSMData unitFSMData = null;

        public override void Initialize(IEntityData data)
        {
            base.Initialize(data);
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

            UpdateForwardDirection();
            UpdateGroundPositionY();
        }

        private void UpdateForwardDirection()
        {
            if (unitMovement == null)
                return;

            if (unitMovement.IsActive == false)
                return;

            if (unitMovement.MovementVelocity.x == 0)
                return;

            int forwardDirection = unitMovement.MovementVelocity.x > 0 ? 1 : -1;
            entityAnimator.SetRotation(forwardDirection > 0);
            unitFSMData.forwardDirection = forwardDirection;
        }

        private void UpdateGroundPositionY()
        {
            if (staticEntity)
                return;

            unitFSMData.groundPositionY = transform.position.y;
        }

        public void SetFloat(bool isFloat)
        {
            unitFSMData.isFloat = isFloat;
            staticEntity = isFloat;
            unitRigidbody.bodyType = isFloat ? RigidbodyType2D.Dynamic : DefaultRigidbodyType;
            unitRigidbody.gravityScale = isFloat ? GameDefine.GRAVITY_SCALE : 0f;
            unitMovement.SetActive(isFloat == false);
        }
    }
}