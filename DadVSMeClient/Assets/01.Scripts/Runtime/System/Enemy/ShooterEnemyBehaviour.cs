using Cysharp.Threading.Tasks;
using DadVSMe.Animals;
using DadVSMe.Enemies.FSM;
using DadVSMe.Entities;
using H00N.Resources.Addressables;
using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe.Enemies
{
    public class ShooterEnemyBehaviour : MonoBehaviour, IPoolableBehaviour
    {
        [SerializeField] Unit unit = null;
        [SerializeField] Transform animalFollowTarget = null;

        [SerializeField] PoolReference poolReference = null;
        public PoolReference PoolReference => poolReference;

        private Animal animal = null;

        private void Awake()
        {
            unit.OnInitializedEvent += InitializeInternal;
        }

        private void InitializeInternal(IEntityData data)
        {
            if(data is ShooterEnemyData shooterEnemyData == false)
                return;

            SpawnAnimalAsync(shooterEnemyData.animalPrefab, shooterEnemyData.animalEntityData).Forget();
        }

        public void OnSpawned() { }
        public void OnDespawn()
        {
            if(animal == null)
                return;

            PoolManager.Despawn(animal);
        }

        private async UniTask SpawnAnimalAsync(AddressableAsset<Animal> animalPrefab, AnimalEntityData animalEntityData)
        {
            await animalPrefab.InitializeAsync();
            animal = PoolManager.Spawn<Animal>(animalPrefab.Key);
            animal.Initialize(animalEntityData);
            animal.SetFollowTarget(animalFollowTarget);

            unit.FSMBrain.GetAIData<ShooterEnemyFSMData>().animal = animal;
            unit.AddChildSortingOrderResolver(animal);
        }
    }
}
