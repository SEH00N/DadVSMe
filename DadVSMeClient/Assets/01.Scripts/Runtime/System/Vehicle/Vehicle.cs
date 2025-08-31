using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe.Entities
{
    public class Vehicle : Unit
    {
        [SerializeField] Transform rideTransform = null;
        [SerializeField] float attackWidth = 6f;
        [SerializeField] float attackHeight = 3f;
        [SerializeField] AttackDataBase attackData = null;

        private IRider rider = null;
        private UnitFSMData riderFSMData = null;

        protected override void InitializeInternal(IEntityData data)
        {
            base.InitializeInternal(data);
            unitHealth.OnHPChangedEvent += HandleOwnerHPChanged;
            _ = new InitializeAttackFeedback(attackData);
        }

        private void FixedUpdate()
        {
            if(riderFSMData == null)
                return;

            riderFSMData.groundPositionY = unitFSMData.groundPositionY;
        }

        public void RideOn(IRider rider)
        {
            this.rider = rider;
            
            rider.transform.SetParent(rideTransform);
            rider.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            rider.transform.localScale = Vector3.one;
            rider.RideOn(this);

            if(rider is Unit riderUnit == true)
            {
                AddChildSortingOrderResolver(riderUnit);
                riderFSMData = riderUnit.FSMBrain.GetAIData<UnitFSMData>();
            }

            unitHealth.OnHPChangedEvent += HandleOwnerHPChanged;
        }

        public void RideOff()
        {
            unitHealth.OnHPChangedEvent -= HandleOwnerHPChanged;

            if(rider is Unit riderUnit == true)
                RemoveChildSortingOrderResolver(riderUnit);

            rider.transform.SetParent(null);
            rider.RideOff(this);

            rider = null;
        }

        private void HandleOwnerHPChanged()
        {
            // if get damaged. it should be explode
            if(rider == null)
                return;

            Explosion();
            RideOff();

            PoolManager.Despawn(this);
        }

        private void Explosion()
        {
            EAttackAttribute attackAttribute = unitFSMData.attackAttribute;
            unitFSMData.attackAttribute = EAttackAttribute.Fire;

            _ = new PlayAttackFeedback(attackData, unitFSMData.attackAttribute, transform.position, Vector3.zero, unitFSMData.forwardDirection);

            Collider2D[] cols = Physics2D.OverlapBoxAll(transform.position, new Vector2(attackWidth, attackHeight), 0, GameDefine.PLAYER_LAYER_MASK);
            foreach (var col in cols)
            {
                if(col.gameObject == rider.transform.gameObject || col.gameObject == gameObject)
                    continue;

                if (col.TryGetComponent<IHealth>(out IHealth health))
                {
                    health.Attack(this, attackData);
                    _ = new PlayHitFeedback(attackData, unitFSMData.attackAttribute, health.Position, Vector3.zero, unitFSMData.forwardDirection);
                }
            }

            unitFSMData.attackAttribute = attackAttribute;
        }

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, new Vector3(attackWidth, attackHeight, 0));
        }
        #endif
    }
}