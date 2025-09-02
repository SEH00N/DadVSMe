using UnityEngine;

namespace DadVSMe.Entities
{
    public interface IRider
    {
        public Transform transform { get; }
        public UnitHealth UnitHealth { get; }

        public void RideOn(Vehicle vehicle);
        public void RideOff();
    }
}