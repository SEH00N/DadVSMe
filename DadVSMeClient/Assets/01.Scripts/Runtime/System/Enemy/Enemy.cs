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

        private bool skipUpdate = false;

        [SerializeField]
        private UnitData unitDataRef;

        private void Awake()
        {
            poolReference = GetComponent<PoolReference>();
        }

        // Debug
        private void Start()
        {
            Initialize(unitDataRef);
        }

        public override void Initialize(IEntityData data)
        {
            base.Initialize(data);
            enemyDetector.Initialize();
        }

        protected override void LateUpdate()
        {
            if(skipUpdate)
                return;

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

        public void OnSpawned()
        {

        }

        public void OnDespawn()
        {
            onDespawned?.Invoke(this);
        }
    }
}
