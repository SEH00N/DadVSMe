using ShibaInspector.Attributes;
using UnityEngine;

namespace DadVSMe.Enemies
{
    [CreateAssetMenu(menuName = "DadVSMe/EnemyData/SimpleEnemyData")]
    public class SimpleEnemyData : EnemyDataBase, IKidEnemyData
    {
        public ESimpleEnemyType enemyType;

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
