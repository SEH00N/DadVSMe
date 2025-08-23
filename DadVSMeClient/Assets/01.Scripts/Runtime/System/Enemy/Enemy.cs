using System;
using Cysharp.Threading.Tasks;
using DadVSMe.Enemies.FSM;
using DadVSMe.Entities;
using DadVSMe.Players;
using H00N.AI.FSM;
using H00N.Resources.Addressables;
using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe.Enemies
{
    public class Enemy : Unit, IGrabbable
    {
        public event Action<Enemy> onDespawned;

        [Header("Enemy")]
        [SerializeField] EnemyDetector enemyDetector = null;
        [SerializeField] FSMState grabState = null;
        [SerializeField] AddressableAsset<Experience> experiencePrefab;

        [Header("Debug")]
        [SerializeField] EnemyDataBase enemyData = null;

        private bool skipUpdate = false;

        // Debug
        private void Start()
        {
            Initialize(enemyData);
            fsmBrain.GetAIData<EnemyFSMData>().player = GameObject.FindFirstObjectByType<Player>(); // TODO: Make GameInstance and Get Player Instance from GameInstane
        }

        protected override void InitializeInternal(IEntityData data)
        {
            base.InitializeInternal(data);
            enemyDetector.Initialize();
            experiencePrefab.InitializeAsync().Forget();
        }

        protected override void LateUpdate()
        {
            if (skipUpdate)
            {
                skipUpdate = false;
                return;
            }

            base.LateUpdate();
        }

        void IGrabbable.Grab(Entity performer)
        {
            staticEntity = true;
            performer.AddChildSortingOrderResolver(sortingOrderResolver);
            fsmBrain.ChangeState(grabState);
        }

        void IGrabbable.Release(Entity performer)
        {
            staticEntity = false;
            skipUpdate = true;
            performer.RemoveChildSortingOrderResolver(sortingOrderResolver);
        }

        protected override void DespawnInternal()
        {
            onDespawned?.Invoke(this);
            base.DespawnInternal();
        }
    }
}
