using Cysharp.Threading.Tasks;
using DadVSMe.Entities;
using DadVSMe.Players.FSM;
using UnityEngine;
using UnityEngine.Events;

namespace DadVSMe.Players
{
    public class Player : Unit
    {
        [Header("Player")]
        [SerializeField] EnemyDetector enemyDetector = null;

        protected override RigidbodyType2D DefaultRigidbodyType => RigidbodyType2D.Dynamic;

        private PlayerFSMData playerFSMData = null;

        public UnityEvent<int> onLevelUpEvent;

        // Debug
        private void Start()
        {
            Initialize(new PlayerEntityData());
        }

        protected override void InitializeInternal(IEntityData data)
        {
            base.InitializeInternal(data);
            enemyDetector.Initialize();

            onAttackTargetEvent.AddListener(OnAttackTarget);
            onStartAngerEvent.AddListener(OnStartAnger);

            playerFSMData = fsmBrain.GetAIData<PlayerFSMData>();
        }

        private void OnAttackTarget(Unit target, IAttackData attackData)
        {
            if (playerFSMData.isAnger)
                return;
            
            AttackDataBase data = attackData as AttackDataBase;
            if(data.IsRangeAttack)
                playerFSMData.currentAngerGauge = Mathf.Min(playerFSMData.currentAngerGauge + 5, playerFSMData.maxAngerGauge);
        }

        public void OnStartAnger()
        {
            playerFSMData.currentAngerGauge = 0f;
        }

        public async void ActiveAngerForUnityEvent()
        {
            await ActiveAnger();
        }

        public async UniTask ActiveAnger()
        {
            playerFSMData.isAnger = true;
            unitStatData[EUnitStat.MoveSpeed].RegistAddModifier(10);
            unitStatData[EUnitStat.AttackPowerMultiplier].RegistAddModifier(0.5f);
            unitFSMData.attackAttribute = EAttackAttribute.Crazy;
            onStartAngerEvent?.Invoke();

            await UniTask.Delay(System.TimeSpan.FromSeconds(playerFSMData.angerTime));

            unitStatData[EUnitStat.MoveSpeed].UnregistAddModifier(10);
            unitStatData[EUnitStat.AttackPowerMultiplier].UnregistAddModifier(0.5f);
            unitFSMData.attackAttribute = EAttackAttribute.Normal;
            playerFSMData.isAnger = false;
            onEndAngerEvent?.Invoke();
        }

        public void GetExp(int amount)
        {
            playerFSMData.currentExp += amount;

            if (playerFSMData.levelUpExp <= playerFSMData.currentExp)
            {
                LevelUp();
            }
        }

        public void LevelUp()
        {
            playerFSMData.currentLevel++;
            playerFSMData.currentExp -= playerFSMData.levelUpExp;
            playerFSMData.levelUpExp =
                Mathf.RoundToInt(playerFSMData.baseLevelUpXP * Mathf.Pow(playerFSMData.levelUpRatio, playerFSMData.currentLevel - 1));

            onLevelUpEvent?.Invoke(playerFSMData.currentLevel);
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                interactable.Interact(this);
            }
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
