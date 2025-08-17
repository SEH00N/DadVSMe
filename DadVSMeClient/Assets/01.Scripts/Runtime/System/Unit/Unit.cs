using UnityEngine;
using H00N.AI.FSM;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;

namespace DadVSMe.Entities
{
    public class Unit : Entity
    {
        [Header("Unit")]
        [SerializeField] protected UnitMovement unitMovement = null;
        [SerializeField] protected FSMBrain fsmBrain = null;
        [SerializeField] protected UnitHealth unitHealth = null;
        [SerializeField] protected UnitAttackEventListener unitAttackEventListener = null;
        [SerializeField] protected UnitSkillComponent unitSkillComponent;
        [SerializeField] protected Rigidbody2D unitRigidbody = null;

        public FSMBrain FSMBrain => fsmBrain;
        public UnitHealth UnitHealth => unitHealth; // uniy health is used frequently. allow external access for performance. 

        protected virtual RigidbodyType2D DefaultRigidbodyType => RigidbodyType2D.Kinematic;

        [SerializeField]
        protected UnitData unitDataRef;
        protected UnitData unitData = null;
        public UnitData UnitData => unitData;
        private UnitFSMData unitFSMData = null;

        protected EAttackAttribute attackAttribute;

        public UnityEvent<Unit, IAttackData> onAttackTargetEvent = null;
        public UnityEvent onStartAngerEvent;
        public UnityEvent onEndAngerEvent;

        public override void Initialize(IEntityData data)
        {
            base.Initialize(data);
            unitData = Instantiate<UnitData>(data as UnitData);
            unitData.Initiallize();
            fsmBrain.Initialize();
            fsmBrain.SetAsDefaultState();
            unitHealth.Initialize(unitData.Stat[EUnitStat.MaxHp]);
            unitAttackEventListener.Initialize();
            unitSkillComponent?.Initialize();

            attackAttribute = EAttackAttribute.Normal;

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

        public void SetAttackAttribute(EAttackAttribute attackAttribute)
        {
            this.attackAttribute = attackAttribute;
            if (unitData.attackAttribute != EAttackAttribute.Crazy)
                unitData.attackAttribute = attackAttribute;
        }

         public async void ActiveAngerForUnityEvent()
        {
            await ActiveAnger();
        }

        public async UniTask ActiveAnger()
        {
            unitData.isAnger = true;
            unitData.Stat[EUnitStat.MoveSpeed].RegistAddModifier(10);
            unitData.Stat[EUnitStat.AttackPowerMultiplier].RegistAddModifier(0.5f);
            unitData.attackAttribute = EAttackAttribute.Crazy;
            onStartAngerEvent?.Invoke();

            await UniTask.Delay(System.TimeSpan.FromSeconds(unitData.angerTime));

            unitData.Stat[EUnitStat.MoveSpeed].UnregistAddModifier(10);
            unitData.Stat[EUnitStat.AttackPowerMultiplier].UnregistAddModifier(0.5f);
            unitData.attackAttribute = attackAttribute;
            unitData.isAnger = false;
            onEndAngerEvent?.Invoke();
        }
    }
}