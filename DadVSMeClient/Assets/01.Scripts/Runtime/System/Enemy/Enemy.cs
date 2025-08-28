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
        [SerializeField] NPCMovement npcMovement = null;
        [SerializeField] EnemyDetector enemyDetector = null;
        [SerializeField] FSMState grabState = null;

        [field: SerializeField]
        public float Weight { get; set; }

        private bool skipUpdate = false;

        protected override void InitializeInternal(IEntityData data)
        {
            base.InitializeInternal(data);
            npcMovement.Initialize();
            enemyDetector.Initialize();
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
