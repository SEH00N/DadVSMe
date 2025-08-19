using DadVSMe.Animals;
using H00N.Resources.Addressables;
using ShibaInspector.Attributes;
using UnityEngine;

namespace DadVSMe.Enemies
{
    // [CreateAssetMenu(menuName = "DadVSMe/EntityData/KidEnemyData")]
    public abstract class KidEnemyData : EnemyDataBase
    {
        [Header("Kid Enemy")]
        public Sprite hatSprite;
        public Sprite clothesSprite;
        
        public bool useBodyColorOverride;
        [ConditionalField("useBodyColorOverride", true, true)] public Color bodyColorOverride;
    }
}
