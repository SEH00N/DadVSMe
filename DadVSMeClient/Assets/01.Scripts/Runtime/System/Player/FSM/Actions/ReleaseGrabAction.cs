using UnityEngine;
using DadVSMe.Entities;
using H00N.AI.FSM;

namespace DadVSMe.Players.FSM
{
    public class ReleaseGrabAction : FSMAction
    {
        [SerializeField] Collider2D defaultSortingOrderResolverCollider = null;
        [SerializeField] Collider2D grabbedSortingOrderResolverCollider = null;

        private PlayerFSMData fsmData = null;
        private UnitFSMData unitFSMData = null;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            fsmData = brain.GetAIData<PlayerFSMData>();
            unitFSMData = brain.GetAIData<UnitFSMData>();
        }

        public override void EnterState()
        {
            base.EnterState();

            if(fsmData.grabbedEntity != null)
            {
                Entity grabbedEntity = fsmData.grabbedEntity;
                fsmData.grabbedEntity = null;

                grabbedEntity.transform.SetParent(null);
                (grabbedEntity as IGrabbable).Release(unitFSMData.unit);
                (grabbedEntity as Unit).FSMBrain.GetAIData<UnitFSMData>().groundPositionY = unitFSMData.groundPositionY;
                grabbedEntity.transform.position = new Vector3(grabbedEntity.transform.position.x, unitFSMData.groundPositionY, grabbedEntity.transform.position.z);
                if(grabbedEntity.TryGetComponent<FSMBrain>(out FSMBrain grabbedEntityFSMBrain))
                    grabbedEntityFSMBrain.SetAsDefaultState();
            }

            defaultSortingOrderResolverCollider.enabled = true;
            grabbedSortingOrderResolverCollider.enabled = false;

            brain.SetAsDefaultState();
        }
    }
}
