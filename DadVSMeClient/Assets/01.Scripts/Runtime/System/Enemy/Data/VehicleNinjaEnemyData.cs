using DadVSMe.Entities;
using H00N.Resources.Addressables;
using UnityEngine;

namespace DadVSMe.Enemies
{
    [CreateAssetMenu(menuName = "DadVSMe/EntityData/VehicleNinjaEnemyData")]
    public class VehicleNinjaEnemyData : NinjaData, IVehicleEnemyData
    {
        [SerializeField] AddressableAsset<Vehicle> vehiclePrefab = null;
        public AddressableAsset<Vehicle> VehiclePrefab => vehiclePrefab;
    }
}