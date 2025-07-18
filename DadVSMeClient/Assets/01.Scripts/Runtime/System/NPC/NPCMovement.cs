using UnityEngine;
using UnityEngine.AI;

namespace DadVSMe.NPCs
{
    [RequireComponent(typeof(EntityMovement))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class NPCMovement : MonoBehaviour
    {
        private NavMeshAgent navMeshAgent;
        private EntityMovement entityMovement;

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            entityMovement = GetComponent<EntityMovement>();
        }

        private void Start()
        {
            navMeshAgent.updateRotation = false;
            navMeshAgent.updateUpAxis = false;
        }

        private void Update()
        {
            if (navMeshAgent.pathPending || navMeshAgent.hasPath == false || navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                entityMovement.SetMovementVelocity(Vector2.zero);
                return;
            }

            Vector3 direction = navMeshAgent.steeringTarget - transform.position;
            float distance = direction.magnitude;

            Vector3 normalizedDirection = direction.normalized;
            float adjustedSpeed = Mathf.Min(navMeshAgent.speed, distance / Time.fixedDeltaTime);

            Vector3 targetVelocity = normalizedDirection * adjustedSpeed;
            entityMovement.SetMovementVelocity(targetVelocity);
        }

        public void SetDestination(Vector2 destination)
        {
            navMeshAgent.SetDestination(destination);
        }
    }
}
