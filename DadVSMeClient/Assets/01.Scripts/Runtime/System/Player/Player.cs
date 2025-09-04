using System;
using DadVSMe.Entities;
using DadVSMe.Players.FSM;
using UnityEngine;
using UnityEngine.Events;

namespace DadVSMe.Players
{
    public class Player : Unit
    {
        private const float ANGER_GAUGE_DECREMENT_THRESHOLD = 1f;

        [Header("Player")]
        [SerializeField] EnemyDetector enemyDetector = null;
        [SerializeField] PlayerItemCollector itemCollector = null;
        [SerializeField] GameObject powerUpEffect = null;

        private PlayerFSMData playerFSMData = null;

        public UnityEvent<int> onLevelUpEvent;
        public event Action OnEXPChangedEvent = null;

        private float lastAngerGauseModifiedTime = 0f;

        //private void Start()
        //{
        //    Initialize(new PlayerEntityData());
        //}

        protected override void InitializeInternal(IEntityData data)
        {
            base.InitializeInternal(data);

            itemCollector.Initialize(this);
            enemyDetector.Initialize();

            unitHealth.onAttackEvent.AddListener(HandleGetAttacked);
            onAttackTargetEvent.AddListener(OnAttackTarget);

            playerFSMData = fsmBrain.GetAIData<PlayerFSMData>();
        }

        private void HandleGetAttacked(IAttacker attacker, IAttackData attackData)
        {
            if(attackData.Damage <= 0f)
                return;
            
            DeactiveAnger();
        }

        private void OnAttackTarget(Unit target, IAttackData attackData)
        {
            if(attackData is AttackDataBase data == false)
                return;

            if (data.IsRageAttack == false)
                return;

            playerFSMData.currentAngerGauge = Mathf.Min(playerFSMData.currentAngerGauge + GameDefine.ANGER_GAUGE_INCREMENT, playerFSMData.maxAngerGauge);
            lastAngerGauseModifiedTime = Time.time;
        }

        protected override void Update()
        {
            base.Update();

            // if(playerFSMData.isAnger == false)

            if(playerFSMData.currentAngerGauge <= 0f)
                return;

            if(Time.time - lastAngerGauseModifiedTime < ANGER_GAUGE_DECREMENT_THRESHOLD)
                return;
            
            playerFSMData.currentAngerGauge = Mathf.Max(playerFSMData.currentAngerGauge - GameDefine.ANGER_GAUGE_DECREMENT * Time.deltaTime, 0f);
            if(playerFSMData.currentAngerGauge <= 0f && playerFSMData.isAnger)
                DeactiveAnger();
        }

        public void ActiveAnger()
        {
            playerFSMData.isAnger = true;
            unitStatData[EUnitStat.MoveSpeed].RegistAddModifier(unitStatData[EUnitStat.AngerMoveSpeedModifier].FinalValue);
            unitStatData[EUnitStat.AttackPowerMultiplier].RegistAddModifier(unitStatData[EUnitStat.AttackPowerMultiplierModifier].FinalValue);
            unitFSMData.attackAttribute = EAttackAttribute.Crazy;
            powerUpEffect.SetActive(true);
        }

        private void DeactiveAnger()
        {
            unitStatData[EUnitStat.MoveSpeed].UnregistAddModifier(unitStatData[EUnitStat.AngerMoveSpeedModifier].FinalValue);
            unitStatData[EUnitStat.AttackPowerMultiplier].UnregistAddModifier(unitStatData[EUnitStat.AttackPowerMultiplierModifier].FinalValue);
            unitFSMData.attackAttribute = EAttackAttribute.Normal;
            playerFSMData.isAnger = false;
            playerFSMData.currentAngerGauge = 0f;
            powerUpEffect.SetActive(false);
        }

        public void GetExp(int amount)
        {
            playerFSMData.currentExp += amount;

            while (playerFSMData.levelUpExp <= playerFSMData.currentExp)
                LevelUp();

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
