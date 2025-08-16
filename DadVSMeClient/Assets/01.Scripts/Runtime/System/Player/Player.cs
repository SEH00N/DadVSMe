using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DadVSMe.Entities;
using UnityEngine;

namespace DadVSMe.Players
{
    public class Player : Unit
    {
        [Header("Player")]
        [SerializeField] EnemyDetector enemyDetector = null;

        protected override RigidbodyType2D DefaultRigidbodyType => RigidbodyType2D.Dynamic;

        private bool isAnger;
        public bool IsAnger
        {
            get => isAnger;

            set
            {
                isAnger = value;
            }
        }
        [SerializeField]
        private float maxAngerGauge;
        public float MaxAngerGauge
        {
            get => maxAngerGauge;

            set
            {
                maxAngerGauge = value;
            }
        }
        private float currentAngerGauge;
        public float CurrentAngerGauge
        {
            get => currentAngerGauge;

            set
            {
                currentAngerGauge = value;
            }
        }
        [SerializeField]
        private float angerTime;

        [SerializeField]
        private UnitData unitDataRef;

        // Debug
        private void Start()
        {
            Initialize(unitDataRef);
        }

        public override void Initialize(IEntityData data)
        {
            base.Initialize(data);
            enemyDetector.Initialize();

            onAttackTargetEvent.AddListener(OnAttackTarget);
        }

        private void OnAttackTarget(Unit target, IAttackData attackData)
        {
            if (isAnger)
                return;
            AttackDataBase data = attackData as AttackDataBase;

            if(data.IsRangeAttack)
                CurrentAngerGauge = Mathf.Min(CurrentAngerGauge + 5, maxAngerGauge);
        }

        public async void ActiveAngerForUnityEvent()
        {
            await ActiveAnger();
        }

        public async UniTask ActiveAnger()
        {
            IsAnger = true;

            await UniTask.Delay(System.TimeSpan.FromSeconds(angerTime));

            IsAnger = false;
        }

        #if UNITY_EDITOR
        [ContextMenu("Set FSM State as Default State")]
        private void SetFSMStateAsDefaultState()
        {
            fsmBrain.SetAsDefaultState();
        }
        #endif
    }
}
