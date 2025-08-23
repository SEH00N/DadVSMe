using DadVSMe.Animals;
using DadVSMe.Enemies;
using H00N.Resources.Addressables;
using UnityEngine;

namespace DadVSMe
{
    [CreateAssetMenu(fileName = "SunchipsEnemyData", menuName = "DadVSMe/EntityData/SunchipsEnemyData")]
    public class SunchipsEnemyData : EnemyDataBase
    {
        [Header("Shooter Enemy")]
        public AddressableAsset<Animal> animalPrefab;
        public AnimalEntityData animalEntityData;
        public float shootCooltime = 1f;
        public float buttCooltime = 1f;
    }
}
