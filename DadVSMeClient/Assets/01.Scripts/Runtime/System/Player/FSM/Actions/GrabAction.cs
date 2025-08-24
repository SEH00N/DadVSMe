using DadVSMe.Enemies;
using DadVSMe.Entities;
using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe.Players.FSM
{
    public class GrabAction : FSMAction
    {
        [SerializeField] Collider2D defaultSortingOrderResolverCollider = null;
        [SerializeField] Collider2D grabbedSortingOrderResolverCollider = null;
        private PlayerFSMData playerFSMData = null;
        private UnitFSMData unitFSMData = null;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            playerFSMData = brain.GetAIData<PlayerFSMData>();
            unitFSMData = brain.GetAIData<UnitFSMData>();
        }

        public override void EnterState()
        {
            base.EnterState();

            if (playerFSMData.grabbedEntity != null)
                return;

            Unit enemy = unitFSMData.enemies[0];
            if (enemy.TryGetComponent<Entity>(out Entity entity) == false)
            {
                brain.SetAsDefaultState();
                return;
            }

            if (entity.TryGetComponent<IGrabbable>(out IGrabbable grabbable) == false)
            {
                brain.SetAsDefaultState();
                return;
            }

            grabbable.Grab(unitFSMData.unit);

            playerFSMData.grabParent.localPosition = playerFSMData.grabPosition.localPosition;

            playerFSMData.grabbedEntity = entity;
            playerFSMData.grabbedEntity.transform.SetParent(playerFSMData.grabParent);
            playerFSMData.grabbedEntity.transform.localPosition = Vector3.zero;
            playerFSMData.grabbedEntity.transform.localScale = new Vector3(-1, 1, 1);

            defaultSortingOrderResolverCollider.enabled = false;
            grabbedSortingOrderResolverCollider.enabled = true;

            playerFSMData.onGrabbedEntityChanged?.Invoke(entity);
        }
    }
}
