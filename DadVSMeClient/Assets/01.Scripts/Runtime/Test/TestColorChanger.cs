using ShibaInspector.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace DadVSMe
{
    public class TestColorChanger : MonoBehaviour
    {
        [SerializeField] Image image = null;
        [SerializeField] Color baseColor = Color.white;
        [SerializeField] Color targetColor = Color.white;

        [ContextMenu("Change Color")]
        public void ChangeColor()
        {
            image.color = GetColorRatio(baseColor, targetColor);
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
