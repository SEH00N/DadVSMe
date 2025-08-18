using DadVSMe.Animals;
using H00N.Resources.Addressables;
using ShibaInspector.Attributes;
using UnityEngine;

namespace DadVSMe.Enemies
{
    [CreateAssetMenu(menuName = "DadVSMe/EntityData/SimpleEnemyData")]
    public class SimpleEnemyData : EnemyDataBase, IKidEnemyData
    {
        public ESimpleEnemyType enemyType;

        [ConditionalField("enemyType", ESimpleEnemyType.Shooting, true)] public AddressableAsset<Animal> animalPrefab;
        [ConditionalField("enemyType", ESimpleEnemyType.Shooting, true)] public AnimalEntityData animalEntityData;

        [SerializeField] Sprite hatSprite;
        public Sprite HatSprite => hatSprite;

        [SerializeField] Sprite clothesSprite;
        public Sprite ClothesSprite => clothesSprite;
        
        [SerializeField] bool useBodyColorOverride;
        public bool UseBodyColorOverride => useBodyColorOverride;

        [ConditionalField("useBodyColorOverride", true, true), SerializeField] Color bodyColorOverride;
        public Color BodyColorOverride => bodyColorOverride;
    }
}
