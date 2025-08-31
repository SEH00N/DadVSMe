using DadVSMe.Entities;
using Cysharp.Threading.Tasks;
using H00N.Resources.Addressables;
using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe.Enemies
{
    public class VehicleEnemyBehaviour : MonoBehaviour
    {
        [SerializeField] Unit unit = null;
        
        private void Awake()
        {
            unit.OnInitializedEvent += HandleInitialized;
        }

        private void HandleInitialized(IEntityData data)
        {
            if (data is IVehicleEnemyData vehicleEnemyData == false)
                return;
            
            if(unit is IRider rider == false)
                return;

            unit.SetHold(true);
            SpawnVehicleAsync(vehicleEnemyData.VehiclePrefab, rider).Forget();
        }

        private async UniTask SpawnVehicleAsync(AddressableAsset<Vehicle> vehiclePrefab, IRider rider)
        {
            await vehiclePrefab.InitializeAsync();
            Vehicle vehicle = PoolManager.Spawn<Vehicle>(vehiclePrefab.Key);
            vehicle.transform.position = unit.transform.position;
            vehicle.Initialize(new VehicleEntityData());

            unit.SetHold(false);
            vehicle.RideOn(rider);
        }
    }
}