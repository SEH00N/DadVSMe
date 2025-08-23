using DadVSMe.Entities;
using H00N.AI.FSM;
using H00N.Resources.Addressables;
using UnityEngine;
using System.Collections.Generic;
using H00N.Resources.Pools;
using Cysharp.Threading.Tasks;

namespace DadVSMe.Enemies.FSM
{
    public class NinjaAttackAction : FSMAction
    {
        [SerializeField] AttackDataBase attackData;
        [SerializeField] float kickSpeed;
        [SerializeField] float targetThresHold = 0.1f;
        [SerializeField] Vector2 attackOffset;

        [Header("Attack After Image")]
        [SerializeField] AddressableAsset<NinjaAfterImage> ninjaAfterImagePrefab;
        [SerializeField] float spawnRate;
        [SerializeField] Vector2 spawnOffset;
        private float currentTime;
        private List<NinjaAfterImage> spawnedAfterImageContainer;

        private Vector2 kickDirection;
        private Vector2 targetPosition;

        private UnitFSMData unitFSMData = null;
        private EnemyFSMData enemyFSMData = null;
        private UnitMovement unitMovement = null;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);

            unitFSMData = brain.GetAIData<UnitFSMData>();
            enemyFSMData = brain.GetAIData<EnemyFSMData>();
            unitMovement = brain.GetComponent<UnitMovement>();

            spawnedAfterImageContainer = new List<NinjaAfterImage>();

            ninjaAfterImagePrefab.InitializeAsync().Forget();
        }

        public override void EnterState()
        {
            base.EnterState();

            unitMovement.SetActive(true);

            targetPosition = enemyFSMData.player.transform.position;
            kickDirection = (targetPosition - (Vector2)brain.transform.position).normalized;

            currentTime = 0;
        }

        public override void UpdateState()
        {
            base.UpdateState();

            unitMovement.SetMovementVelocity(kickDirection * kickSpeed);

            var position = brain.transform.position;
            var target = enemyFSMData.player;

            if(Vector2.Distance(position, target.transform.position) < targetThresHold)
            {
                attackData.attackAttribute = unitFSMData.attackAttribute;
                target.UnitHealth.Attack(unitFSMData.unit, attackData);
                unitFSMData.unit.onAttackTargetEvent?.Invoke(target, attackData);

                _ = new PlayAttackFeedback(attackData, unitFSMData.attackAttribute, target.transform.position, attackOffset, unitFSMData.forwardDirection);

                brain.SetAsDefaultState();
                return;
            }

            if (Vector2.Distance(brain.transform.position, targetPosition) < targetThresHold)
            {
                brain.SetAsDefaultState();
                return;
            }

            if(currentTime >= spawnRate)
            {
                var forward = unitFSMData.forwardDirection;

                var afterImage = PoolManager.Spawn<NinjaAfterImage>(ninjaAfterImagePrefab);
                afterImage.Initialize((Vector2)brain.transform.position + spawnOffset, forward);
                spawnedAfterImageContainer.Add(afterImage);

                currentTime = 0;
            }

            currentTime += Time.deltaTime;
        }

        public override void ExitState()
        {
            base.ExitState();

            unitFSMData.unit.SetFloat(false);
            unitMovement.SetActive(false);
        }
    }
}
