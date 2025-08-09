using UnityEngine;
using UnityEngine.AI;

namespace DadVSMe.Entities
{
    [RequireComponent(typeof(UnitMovement))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class NPCMovement : MonoBehaviour
    {
        private NavMeshAgent navMeshAgent;
        private UnitMovement unitMovement;

        private bool isActive = false;
        public bool IsActive => isActive;

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            unitMovement = GetComponent<UnitMovement>();
        }

        private void Start()
        {
            navMeshAgent.updateRotation = false;
            navMeshAgent.updateUpAxis = false;
        }

        private void Update()
        {
            if(isActive == false)
                return;

            if (navMeshAgent.pathPending || navMeshAgent.hasPath == false || navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                unitMovement.SetMovementVelocity(Vector2.zero);
                return;
            }

            Vector3 direction = navMeshAgent.steeringTarget - transform.position;
            float distance = direction.magnitude;

            Vector3 normalizedDirection = direction.normalized;
            float adjustedSpeed = Mathf.Min(navMeshAgent.speed, distance / Time.fixedDeltaTime);

            Vector3 targetVelocity = normalizedDirection * adjustedSpeed;
            unitMovement.SetMovementVelocity(targetVelocity);
        }

        public void SetActive(bool isActive)
        {
            this.isActive = isActive;
            navMeshAgent.enabled = isActive;
            unitMovement.SetActive(isActive);
        }

        public void SetDestination(Vector2 destination)
        {
            navMeshAgent.SetDestination(destination);
        }
    }
}
