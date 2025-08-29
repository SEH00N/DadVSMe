using NavMeshPlus.Components;
using UnityEngine;

namespace DadVSMe.GameCycles
{
    public class GroundBehaviour : MonoBehaviour
    {
        [SerializeField] NavMeshSurface navMeshSurface = null;

        private void Start()
        {
            navMeshSurface.BuildNavMeshAsync();
        }
    }
}
