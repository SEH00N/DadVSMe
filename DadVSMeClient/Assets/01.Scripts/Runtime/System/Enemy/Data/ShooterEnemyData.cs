using DadVSMe.Animals;
using H00N.Resources.Addressables;
using UnityEngine;

namespace DadVSMe.Enemies
{
    [CreateAssetMenu(menuName = "DadVSMe/EntityData/ShooterEnemyData")]
    public class ShooterEnemyData : KidEnemyData
    {
        [Header("Shooter Enemy")]
        public AddressableAsset<Animal> animalPrefab;
        public AnimalEntityData animalEntityData;
        public float shootCooltime = 1f;
    }
}
