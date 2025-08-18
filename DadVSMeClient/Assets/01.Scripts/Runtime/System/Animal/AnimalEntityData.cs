using DadVSMe.Entities;
using H00N.Resources.Addressables;
using UnityEngine;

namespace DadVSMe.Animals
{
    [CreateAssetMenu(menuName = "DadVSMe/EntityData/AnimalEntityData")]
    public class AnimalEntityData : ScriptableObject, IEntityData
    {
        [SerializeField] AddressableAsset<Projectile> projectilePrefab = null;
        public AddressableAsset<Projectile> ProjectilePrefab => projectilePrefab;
    }
}