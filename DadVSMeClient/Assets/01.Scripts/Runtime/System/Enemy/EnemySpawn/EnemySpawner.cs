using DadVSMe.Enemies;
using H00N.Resources.Addressables;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace DadVSMe
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] AddressableAsset<Enemy> _enemyPrefab;

        private async UniTask SpawnEnemy()
        {
            if (_enemyPrefab.Initialized == false)
            {
                await _enemyPrefab.InitializeAsync();
            }


        }
    }
}
