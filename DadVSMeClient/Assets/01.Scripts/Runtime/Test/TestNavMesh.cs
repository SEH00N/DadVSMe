using Cysharp.Threading.Tasks;
using NavMeshPlus.Components;
using UnityEngine;

namespace DadVSMe.Tests
{
    public class TestNavMesh : MonoBehaviour
    {
        [SerializeField] NavMeshSurface navMeshSurface = null;

        private async void Start()
        {
            await navMeshSurface.BuildNavMeshAsync();
            Debug.LogError("BuildNavMeshAsync");
        }
    }
}
