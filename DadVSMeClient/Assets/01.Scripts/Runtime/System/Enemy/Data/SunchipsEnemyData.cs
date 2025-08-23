using DadVSMe.Animals;
using H00N.Resources.Addressables;
using UnityEngine;

namespace DadVSMe
{
    [CreateAssetMenu(fileName = "SunchipsEnemyData", menuName = "DadVSMe/EntityData/SunchipsEnemyData")]
    public class SunchipsEnemyData : ScriptableObject
    {
        [Header("Shooter Enemy")]
        public AddressableAsset<Animal> animalPrefab;
        public AnimalEntityData animalEntityData;
        public float shootCooltime = 1f;
    }
}
