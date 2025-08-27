using UnityEngine.AI;

namespace DadVSMe
{
    public static class NavMeshAgentExtensions
    {
        public static void Initialize(this NavMeshAgent navMeshAgent)
        {
            navMeshAgent.enabled = false;
            navMeshAgent.enabled = true;
            navMeshAgent.Warp(navMeshAgent.transform.position);
        }
    }
}