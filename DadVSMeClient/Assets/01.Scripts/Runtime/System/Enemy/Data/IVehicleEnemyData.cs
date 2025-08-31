using DadVSMe.Entities;
using H00N.Resources.Addressables;

namespace DadVSMe.Enemies
{
    public interface IVehicleEnemyData
    {
        public AddressableAsset<Vehicle> VehiclePrefab { get; }
    }
}