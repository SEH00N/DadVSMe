using System;
using Cysharp.Threading.Tasks;
using DadVSMe.Entities;
using H00N.AI.FSM;
using H00N.Resources.Addressables;
using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe
{
    public class Fire : MonoBehaviour
    {
        private static readonly Vector2 SpawnOffset = new Vector2(0f, 1.5f);

        [SerializeField] AddressableAsset<ParticleSystem> particlePrefab;
        [SerializeField] PoolReference poolReference;

        private IAttackData attackData;
        private IAttackFeedbackDataContainer feedbackDataContainer;
        private Unit target;
        private Unit instigator;
        private float burnTime;
        private float attackDelay;
        private UnitHealth targetHealth;
        private float currentBurnTime = 0;
        private bool isBurn = false;
        private int burnCount = 0;
        
        public void Init(Unit instigator, Unit target, IAttackData attackData, IAttackFeedbackDataContainer feedbackDataContainer, float burnTime, float attackDelay)
        {
            this.instigator = instigator;
            this.attackData = attackData;
            this.feedbackDataContainer = feedbackDataContainer;
            this.burnTime = burnTime;
            this.attackDelay = attackDelay;
            this.target = target;
            currentBurnTime = 0;
            isBurn = false;
            burnCount = 0;

            if (target.TryGetComponent<UnitHealth>(out UnitHealth targetHealth))
            {
                if (target.GetComponentInChildren<Fire>() != null)
                    return;
                    
                this.targetHealth = targetHealth;
                transform.SetParent(target.transform);
                transform.localPosition = Vector3.zero;

                StartBurn();
            }
        }

        private void StartBurn()
        {
            SpawnEffectAsync();

            isBurn = true;
        }

        void Update()
        {
            if (isBurn)
            {
                currentBurnTime += Time.deltaTime;

                if (currentBurnTime >= burnTime)
                {
                    EndBurn();
                }

                if (currentBurnTime >= attackDelay * burnCount)
                {
                    burnCount++;
                    targetHealth.Attack(instigator, attackData);
                    UnitFSMData unitFSMData = instigator.GetComponent<FSMBrain>().GetAIData<UnitFSMData>();
                    _ = new PlayHitFeedback(feedbackDataContainer, unitFSMData.attackAttribute, targetHealth.transform.position, Vector3.zero, unitFSMData.forwardDirection);

                }
            }
        }

        private void EndBurn()
        {
            PoolManager.Despawn(poolReference);
        }

        private async void SpawnEffectAsync()
        {
            await particlePrefab.InitializeAsync();

            ParticleSystem particle = PoolManager.Spawn<ParticleSystem>(particlePrefab.Key, transform);
            particle.transform.localPosition = SpawnOffset;
            particle.Play();

            await UniTask.Delay(System.TimeSpan.FromSeconds(burnTime), cancellationToken: destroyCancellationToken);

            PoolManager.Despawn(particle.GetComponent<PoolReference>());
        }
    }
}
