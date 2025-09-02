using System;
using DadVSMe.Entities;
using H00N.AI.FSM;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DadVSMe.Enemies
{
    public class Enemy : Unit, IGrabbable, IRider
    {
        public event Action<Enemy> onDespawned;

        [Header("Enemy")]
        // [SerializeField] NPCMovement npcMovement = null;
        [SerializeField] EnemyDetector enemyDetector = null;
        [SerializeField] FSMState grabState = null;
        [SerializeField] FSMState ridingState = null;

        [field: SerializeField]
        public float Weight { get; set; }

        private Vehicle vehicle = null;
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

        public void RideOn(Vehicle vehicle)
        {
            this.vehicle = vehicle;

            staticEntity = true;
            fsmBrain.ChangeState(ridingState);
            unitRigidbody.bodyType = RigidbodyType2D.Kinematic;
        }

        public void RideOff()
        {
            staticEntity = false;
            skipUpdate = true;
            fsmBrain.SetAsDefaultState();
            unitRigidbody.bodyType = RigidbodyType2D.Dynamic;

            vehicle = null;
        }

        public override void SetHold(bool isHold, params Object[] holders)
        {
            if(vehicle != null)
                vehicle.SetHold(isHold, holders);
            else
                base.SetHold(isHold, holders);
        }
    }
}
