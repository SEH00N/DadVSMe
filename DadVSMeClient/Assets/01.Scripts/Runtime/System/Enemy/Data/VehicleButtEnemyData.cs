using DadVSMe.Entities;
using H00N.Resources.Addressables;
using UnityEngine;

namespace DadVSMe.Enemies
{
    [CreateAssetMenu(menuName = "DadVSMe/EntityData/VehicleButtEnemyData")]
    public class VehicleButtEnemyData : ButtEnemyData, IVehicleEnemyData
    {
        [SerializeField] AddressableAsset<Vehicle> vehiclePrefab = null;
        public AddressableAsset<Vehicle> VehiclePrefab => vehiclePrefab;
    }
}