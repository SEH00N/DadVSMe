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
        // [SerializeField] NPCMovement npcMovement = null;
        [SerializeField] EnemyDetector enemyDetector = null;
        [SerializeField] FSMState grabState = null;

        [field: SerializeField]
        public float Weight { get; set; }

        private Unit grabber = null;
        private bool skipUpdate = false;

        protected override void InitializeInternal(IEntityData data)
        {
            base.InitializeInternal(data);
            // npcMovement.Initialize();
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

            if(grabber != null)
                unitFSMData.groundPositionY = grabber.FSMBrain.GetAIData<UnitFSMData>().groundPositionY;
        }

        void IGrabbable.Grab(Unit performer)
        {
            staticEntity = true;
            grabber = performer;
            performer.AddChildSortingOrderResolver(sortingOrderResolver);
            fsmBrain.ChangeState(grabState);
            unitRigidbody.bodyType = RigidbodyType2D.Kinematic;
        }

        void IGrabbable.Release(Unit performer)
        {
            staticEntity = false;
            skipUpdate = true;
            grabber = null;
            performer.RemoveChildSortingOrderResolver(sortingOrderResolver);
            unitRigidbody.bodyType = RigidbodyType2D.Dynamic;
        }

        protected override void DespawnInternal()
        {
            onDespawned?.Invoke(this);
            base.DespawnInternal();
        }
    }
}
