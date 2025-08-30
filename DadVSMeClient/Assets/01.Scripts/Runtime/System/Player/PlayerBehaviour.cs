using System;
using DadVSMe.Entities;
using DadVSMe.Inputs;
using DadVSMe.Players.FSM;
using H00N.AI.FSM;
using UnityEngine;

namespace DadVSMe
{
    public class PlayerBehaviour : MonoBehaviour
    {
        [SerializeField] UnitHealth health;
        [SerializeField] FSMBrain brain;
        // private PlayerFSMData fsmData = null;
        // private UnitFSMData unitFSMData = null;

        [Space(10f)]
        [Header("Grab")]
        [SerializeField] Collider2D defaultSortingOrderResolverCollider = null;
        [SerializeField] Collider2D grabbedSortingOrderResolverCollider = null;

        // void Awake()
        // {
        //     fsmData = brain.GetAIData<PlayerFSMData>();
        //     unitFSMData = brain.GetAIData<UnitFSMData>();
        // }

        // void Start()
        // {
        //     health.onAttackEvent.AddListener(OnAttackEvent);
        // }
        
        // Move to Each States which HitState and HoldState
        // private void OnAttackEvent(IAttacker attacker, IAttackData attackData)
        // {
        //     if (fsmData.grabbedEntity != null)
        //     {
        //         Entity grabbedEntity = fsmData.grabbedEntity;
        //         fsmData.grabbedEntity = null;

        //         grabbedEntity.transform.SetParent(null);
        //         (grabbedEntity as IGrabbable).Release(unitFSMData.unit);
        //         (grabbedEntity as Unit).FSMBrain.GetAIData<UnitFSMData>().groundPositionY = unitFSMData.groundPositionY;
        //         grabbedEntity.transform.position = new Vector3(grabbedEntity.transform.position.x, unitFSMData.groundPositionY, grabbedEntity.transform.position.z);
        //         if (grabbedEntity.TryGetComponent<FSMBrain>(out FSMBrain grabbedEntityFSMBrain))
        //             grabbedEntityFSMBrain.SetAsDefaultState();
        //     }

        //     defaultSortingOrderResolverCollider.enabled = true;
        //     grabbedSortingOrderResolverCollider.enabled = false;
        // }
    }
}
