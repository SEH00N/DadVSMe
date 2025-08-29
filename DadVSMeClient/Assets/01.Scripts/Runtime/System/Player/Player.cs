using System;
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
        [SerializeField] PlayerItemCollector itemCollector = null;

        // protected override RigidbodyType2D DefaultRigidbodyType => RigidbodyType2D.Dynamic;

        private PlayerFSMData playerFSMData = null;

        public UnityEvent<int> onLevelUpEvent;
        public event Action OnAngerGaugeChangedEvent = null;
        public event Action OnEXPChangedEvent = null;

        // private void Start()
        // {
        //     Initialize(new PlayerEntityData());
        // }

        protected override void InitializeInternal(IEntityData data)
        {
            base.InitializeInternal(data);

            itemCollector.Initialize(this);
            enemyDetector.Initialize();

            onAttackTargetEvent.AddListener(OnAttackTarget);
            onStartAngerEvent.AddListener(OnStartAnger);

            playerFSMData = fsmBrain.GetAIData<PlayerFSMData>();
        }

        private void OnAttackTarget(Unit target, IAttackData attackData)
        {
            if (playerFSMData.isAnger)
                return;

            if(attackData is AttackDataBase data == false)
                return;

            if (data.IsRageAttack == false)
                return;

            playerFSMData.currentAngerGauge = Mathf.Min(playerFSMData.currentAngerGauge + 5, playerFSMData.maxAngerGauge);
            OnAngerGaugeChangedEvent?.Invoke();
        }

        public void OnStartAnger()
        {
            playerFSMData.currentAngerGauge = 0f;
            OnAngerGaugeChangedEvent?.Invoke();
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

            while (playerFSMData.levelUpExp <= playerFSMData.currentExp)
            {
                LevelUp();
            }

            OnEXPChangedEvent?.Invoke();
        }

        private void LevelUp()
        {
            playerFSMData.currentLevel++;
            playerFSMData.currentExp -= playerFSMData.levelUpExp;
            playerFSMData.levelUpExp = Mathf.RoundToInt(playerFSMData.baseLevelUpXP * Mathf.Pow(playerFSMData.levelUpRatio, playerFSMData.currentLevel - 1));

            onLevelUpEvent?.Invoke(playerFSMData.currentLevel);
        }
    }
}
