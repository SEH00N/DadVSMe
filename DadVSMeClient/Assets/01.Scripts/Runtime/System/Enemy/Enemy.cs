using System;
using DadVSMe.Entities;
using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe.Enemies
{
    public class Enemy : Unit, IGrabbable
    {
        public event Action<Enemy> onDespawned;

        [Header("Enemy")]
        [SerializeField] EnemyDetector enemyDetector = null;
        [SerializeField] FSMState grabState = null;

        private bool skipUpdate = false;
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

        protected override void DespawnInternal()
        {
            onDespawned?.Invoke(this);
            base.DespawnInternal();
        }
    }
}
