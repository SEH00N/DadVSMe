using Cysharp.Threading.Tasks;
using DadVSMe.Animals;
using DadVSMe.Enemies.FSM;
using DadVSMe.Entities;
using H00N.AI.FSM;
using H00N.Resources.Addressables;
using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe.Enemies
{
    public class ShooterEnemyBehaviour : MonoBehaviour, IPoolableBehaviour
    {
        [SerializeField] Unit unit = null;
        [SerializeField] Transform animalFollowTarget = null;
        [SerializeField] FSMState grabState = null;
        [SerializeField] FSMState holdState = null;

        [SerializeField] PoolReference poolReference = null;
        public PoolReference PoolReference => poolReference;

        private UnitFSMData unitFSMData = null;
        private EnemyFSMData enemyFSMData = null;
        private ShooterEnemyData shooterEnemyData = null;

        private Animal animal = null;
        private float shootTimer = 0f;

        private void Awake()
        {
            unit.OnInitializedEvent += InitializeInternal;
        }

        private void Update()
        {
            if(animal == null)
                return;
            
            if(
                unitFSMData.isDie || 
                unitFSMData.isFloat || 
                unitFSMData.isLie || 
                unit.FSMBrain.CurrentState == grabState || 
                unit.FSMBrain.CurrentState == holdState || 
                GameInstance.GameCycle.IsBossWave
            )
            {
                shootTimer = 0f;
                return;
            }

            shootTimer += Time.deltaTime;
            if(shootTimer < shooterEnemyData.shootCooltime)
                return;

            shootTimer = 0f;
            animal.Fire(enemyFSMData.Player.transform.position);
        }

        private void InitializeInternal(IEntityData data)
        {
            if(data is ShooterEnemyData shooterEnemyData == false)
                return;

            this.shooterEnemyData = shooterEnemyData;
            unitFSMData = unit.FSMBrain.GetAIData<UnitFSMData>();
            enemyFSMData = unit.FSMBrain.GetAIData<EnemyFSMData>();

            SpawnAnimalAsync(shooterEnemyData.animalPrefab, shooterEnemyData.animalEntityData).Forget();
        }

        public void OnSpawned() { }
        public void OnDespawn()
        {
            if(animal == null)
                return;

            unit.RemoveChildSortingOrderResolver(animal);
            PoolManager.Despawn(animal);
        }

        private async UniTask SpawnAnimalAsync(AddressableAsset<Animal> animalPrefab, AnimalEntityData animalEntityData)
        {
            await animalPrefab.InitializeAsync();
            animal = PoolManager.Spawn<Animal>(animalPrefab.Key, GameInstance.GameCycle.transform);
            animal.transform.position = animalFollowTarget.position;
            animal.Initialize(animalEntityData);
            animal.SetOwner(unit);
            animal.SetFollowTarget(animalFollowTarget);

            unit.AddChildSortingOrderResolver(animal);
        }
    }
}
