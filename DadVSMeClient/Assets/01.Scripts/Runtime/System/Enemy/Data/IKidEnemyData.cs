using UnityEngine;

namespace DadVSMe.Enemies
{
    public interface IKidEnemyData
    {
        public bool UseBodyColorOverride { get; }
        public Color BodyColorOverride { get; }
        public Sprite HatSprite { get; }
        public Sprite ClothesSprite { get; }
    }
}