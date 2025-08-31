using DadVSMe.Entities;
using H00N.Resources.Addressables;
using UnityEngine;

namespace DadVSMe.Enemies
{
    [CreateAssetMenu(menuName = "DadVSMe/EntityData/VehicleShooterEnemyData")]
    public class VehicleShooterEnemyData : ShooterEnemyData, IVehicleEnemyData
    {
        [SerializeField] AddressableAsset<Vehicle> vehiclePrefab = null;
        public AddressableAsset<Vehicle> VehiclePrefab => vehiclePrefab;
    }
}