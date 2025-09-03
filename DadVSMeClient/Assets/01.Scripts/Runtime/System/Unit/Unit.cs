using UnityEngine;
using H00N.AI.FSM;
using UnityEngine.Events;
using System.Collections.Generic;

namespace DadVSMe.Entities
{
    public class Unit : Entity, IAttacker
    {
        [Header("Unit")]
        [SerializeField] protected UnitMovement unitMovement = null;
        [SerializeField] protected FSMBrain fsmBrain = null;
        [SerializeField] protected UnitHealth unitHealth = null;
        [SerializeField] protected UnitSkillComponent unitSkillComponent;
        [SerializeField] protected Rigidbody2D unitRigidbody = null;
        [SerializeField] protected Collider2D unitCollider = null;
        [SerializeField] protected FSMState holdState = null;
        [SerializeField] protected Vector2 size = Vector2.zero;
        [SerializeField] protected FSMState bounceState = null;

        public FSMBrain FSMBrain => fsmBrain;
        public UnitHealth UnitHealth => unitHealth; // uniy health is used frequently. allow external access for performance. 
        public Collider2D UnitCollider => unitCollider;
        public Vector2 Size => size;

        // protected virtual RigidbodyType2D DefaultRigidbodyType => RigidbodyType2D.Kinematic;

        protected UnitFSMData unitFSMData = null;
        protected UnitStatData unitStatData = null;
        public UnitStatData Stat => unitStatData;

        public Transform AttackerTransform => transform;
        public EAttackAttribute AttackAttribute => unitFSMData.attackAttribute;
        public float AttackPower => unitStatData[EUnitStat.AttackPowerMultiplier].FinalValue;

        public UnityEvent<Unit, IAttackData> onAttackTargetEvent = null;
        public UnityEvent onStartAngerEvent;
        public UnityEvent onEndAngerEvent;

        protected override void InitializeInternal(IEntityData data)
        {
            base.InitializeInternal(data);

            fsmBrain.Initialize();
            fsmBrain.SetAsDefaultState();

            unitFSMData = fsmBrain.GetAIData<UnitFSMData>();
            unitStatData = fsmBrain.GetAIData<UnitStatData>();

            unitHealth.Initialize(unitStatData[EUnitStat.MaxHp]);
            unitSkillComponent?.Initialize();

            unitFSMData.isFloat = false;
            unitFSMData.isLie = false;
            unitFSMData.isDie = false;
        }

        protected virtual void Update()
        {
            if(unitFSMData == null)
                return;

            if(unitFSMData.isFloat == false || fsmBrain.CurrentState == bounceState)
                return;

            // if the bounce is not due to a collision, the bounce is checked based on the last ground position.
            if(transform.position.y >= unitFSMData.groundPositionY || unitRigidbody.linearVelocity.y > 0)
                return;

            unitFSMData.collisionData = new UnitCollisionData(unitRigidbody.linearVelocity, Vector2.up, new Vector2(transform.position.x, unitFSMData.groundPositionY));
            fsmBrain.ChangeState(bounceState);
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
            if (StaticEntity)
                return;

            unitFSMData.groundPositionY = transform.position.y;
        }

        public void SetFloat(bool isFloat)
        {
            unitFSMData.isFloat = isFloat;
            staticEntity = isFloat;
            // unitRigidbody.bodyType = isFloat ? RigidbodyType2D.Dynamic : DefaultRigidbodyType;
            unitRigidbody.gravityScale = isFloat ? GameDefine.GRAVITY_SCALE : 0f;
            unitMovement.SetActive(isFloat == false);
            unitCollider.isTrigger = isFloat;
            sortingOrderResolver.SetActive(isFloat == false);
        }

        public void SetAttackAttribute(EAttackAttribute attackAttribute)
        {
            if (unitFSMData.attackAttribute != EAttackAttribute.Crazy)
                unitFSMData.attackAttribute = attackAttribute;
        }

        public virtual void SetHold(bool isHold, params Object[] holders)
        {
            if(holders == null)
                return;

            if(isHold)
            {
                foreach(Object holder in holders)
                    unitFSMData.holders.Add(holder);

                if(unitFSMData.holders.Count > 0)
                    fsmBrain.ChangeState(holdState);
            }
            else
            {
                foreach(Object holder in holders)
                    unitFSMData.holders.Remove(holder);

                if(unitFSMData.holders.Count == 0)
                    fsmBrain.SetAsDefaultState();
            }
        }
    }
}