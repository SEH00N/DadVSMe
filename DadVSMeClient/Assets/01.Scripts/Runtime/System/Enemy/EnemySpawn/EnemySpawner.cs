using DadVSMe.Enemies;
using H00N.Resources.Addressables;
using UnityEngine;
using Cysharp.Threading.Tasks;
using H00N.Resources.Pools;

namespace DadVSMe
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] AddressableAsset<Enemy> _enemyPrefab;
        [SerializeField] EnemySpawnData _enemySpawnData;

        private async UniTask SpawnEnemy()
        {
            if (_enemyPrefab.Initialized == false)
            {
                await _enemyPrefab.InitializeAsync();
            }

            var enemy = PoolManager.Spawn(_enemyPrefab);
        }
    }
}
