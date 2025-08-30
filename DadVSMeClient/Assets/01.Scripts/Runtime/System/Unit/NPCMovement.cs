using UnityEngine;
using UnityEngine.AI;

namespace DadVSMe.Entities
{
    [RequireComponent(typeof(UnitMovement))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class NPCMovement : MonoBehaviour, IMovement
    {
        private NavMeshAgent navMeshAgent;
        private UnitMovement unitMovement;

        private NavMeshPath cachedPath = null;

        private bool isActive = false;
        public bool IsActive => isActive;

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            unitMovement = GetComponent<UnitMovement>();
            cachedPath = new NavMeshPath();
        }

        public void Initialize()
        {
            navMeshAgent.Initialize();
            navMeshAgent.isStopped = true;
            navMeshAgent.updatePosition = false;
            navMeshAgent.updateRotation = false;
            navMeshAgent.updateUpAxis = false;
        }

        private void Update()
        {
            if(isActive == false)
                return;

            bool isPathInvalid = 
                navMeshAgent.pathPending || 
                navMeshAgent.pathStatus == NavMeshPathStatus.PathInvalid ||
                navMeshAgent.hasPath == false || 
                navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance;

            if (isPathInvalid)
            {
                unitMovement.SetMovementVelocity(Vector2.zero);
                return;
            }

            Vector2 direction = navMeshAgent.steeringTarget - transform.position;
            Vector2 targetVelocity = direction.normalized * navMeshAgent.speed;
            unitMovement.SetMovementVelocity(targetVelocity);
            navMeshAgent.nextPosition = transform.position;
        }

        public void SetActive(bool isActive)
        {
            this.isActive = isActive;
            unitMovement.SetActive(isActive);
        }

        public void SetDestination(Vector2 destination)
        {
            navMeshAgent.SetDestination(destination);
        }

        public Vector2 GetValidDestination(Vector2 destination)
        {
            if (navMeshAgent.CalculatePath(destination, cachedPath) && cachedPath.corners.Length > 1)
                return cachedPath.corners[^1];

            return transform.position;
        }
        
        public void SetMoveSpeed(float moveSpeed)
        {
            navMeshAgent.speed = moveSpeed;
        }

        public float GetMoveSpeed()
        {
            return navMeshAgent.speed;
        }
    }
}
