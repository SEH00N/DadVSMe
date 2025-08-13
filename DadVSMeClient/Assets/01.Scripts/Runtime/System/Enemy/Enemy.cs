using System;
using DadVSMe.Entities;
using H00N.AI.FSM;
using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe.Enemies
{
    public class Enemy : Unit, IGrabbable, IPoolableBehaviour
    {
        private PoolReference poolReference;
        public PoolReference PoolReference => poolReference;

        public event Action<Enemy> onDespawned;

        [Header("Enemy")]
        [SerializeField] EnemyDetector enemyDetector = null;
        [SerializeField] FSMState grabState = null;

        private IEnemyData enemyData;
        public IEnemyData DataInfo => enemyData;

        private void Awake()
        {
            poolReference = GetComponent<PoolReference>();
        }

        // Debug
        private void Start()
        {
            Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();
            enemyDetector.Initialize();
        }

        public void Initialize(IEnemyData data)
        {
            enemyData = data;
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
            performer.RemoveChildSortingOrderResolver(sortingOrderResolver);
        }

        public void OnSpawned()
        {

        }

        public void OnDespawn()
        {
            onDespawned?.Invoke(this);
        }
    }
}
