using DadVSMe.Entities;
using DadVSMe.Players.FSM;
using UnityEngine;

namespace DadVSMe.Players
{
    public class Player : Unit
    {
        [Header("Player")]
        [SerializeField] EnemyDetector enemyDetector = null;

        protected override RigidbodyType2D DefaultRigidbodyType => RigidbodyType2D.Dynamic;

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

            onStartAngerEvent.AddListener(OnStartAnger);
        }

        private void OnAttackTarget(Unit target, IAttackData attackData)
        {
            if (unitData.isAnger)
                return;
            
            PlayerFSMData playerFSMData = fsmBrain.GetAIData<PlayerFSMData>();
            AttackDataBase data = attackData as AttackDataBase;
            if(data.IsRangeAttack)
                playerFSMData.currentAngerGauge = Mathf.Min(playerFSMData.currentAngerGauge + 5, playerFSMData.maxAngerGauge);
        }

        public void OnStartAnger()
        {
            PlayerFSMData playerFSMData = fsmBrain.GetAIData<PlayerFSMData>();
            playerFSMData.currentAngerGauge = 0f;
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
