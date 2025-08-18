using DadVSMe.Entities;
using System.Collections.Generic;
using UnityEngine;

namespace DadVSMe.Enemies
{
    public class KidEnemyBehaviour : MonoBehaviour
    {
        private static readonly Color DefaultBodyColor = new Color(0.9960784f, 0.8f, 0.9960784f, 1);

        [SerializeField] Unit unit = null;
        [SerializeField] List<SpriteRenderer> bodyRenderers = null;
        [SerializeField] SpriteRenderer hatRenderer = null;
        [SerializeField] SpriteRenderer clothesRenderer = null;

        private void Awake()
        {
            unit.OnInitializedEvent += InitializeInternal;
        }

        private void InitializeInternal(IEntityData data)
        {
            if (data is not IKidEnemyData kidEnemyData)
                return;

            Color bodyColor = kidEnemyData.UseBodyColorOverride ? GetColorRatio(DefaultBodyColor, kidEnemyData.BodyColorOverride) : Color.white;
            foreach (SpriteRenderer bodyRenderer in bodyRenderers)
                bodyRenderer.color = bodyColor;
            
            hatRenderer.sprite = kidEnemyData.HatSprite;
            clothesRenderer.sprite = kidEnemyData.ClothesSprite;
        }

        private static Color GetColorRatio(Color baseColor, Color targetColor)
        {
            return new Color(
                baseColor.r != 0 ? targetColor.r / baseColor.r : 1f,
                baseColor.g != 0 ? targetColor.g / baseColor.g : 1f,
                baseColor.b != 0 ? targetColor.b / baseColor.b : 1f,
                baseColor.a != 0 ? targetColor.a / baseColor.a : 1f
            );
        }
    }
}
