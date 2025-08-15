using DadVSMe.Enemies;
using DadVSMe.Entities;
using DadVSMe.Entities.FSM;
using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe.Players.FSM
{
    public class ThrowAttackAction : SimpleAttackAction
    {
        [SerializeField] Collider2D defaultSortingOrderResolverCollider = null;
        [SerializeField] Collider2D grabbedSortingOrderResolverCollider = null;

        private PlayerFSMData fsmData = null;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            fsmData = brain.GetAIData<PlayerFSMData>();
        }

        protected override void OnAttack(EntityAnimationEventData eventData)
        {
            if(fsmData.grabbedEntity != null)
            {
                Entity grabbedEntity = fsmData.grabbedEntity;
                fsmData.grabbedEntity = null;

                grabbedEntity.transform.SetParent(null);
                (grabbedEntity as IGrabbable).Release(unitFSMData.unit);
                (grabbedEntity as Unit).FSMBrain.GetAIData<UnitFSMData>().groundPositionY = unitFSMData.groundPositionY;
            }

            defaultSortingOrderResolverCollider.enabled = true;
            grabbedSortingOrderResolverCollider.enabled = false;

            base.OnAttack(eventData);
        }
    }
}
