using UnityEngine;

namespace DadVSMe
{
    public class LetterBox : MonoBehaviour
    {
        private const int ASPECT_RATIO_WIDTH = 16;
        private const int ASPECT_RATIO_HEIGHT = 9;

        private Camera currentCamera;

        private float lastWidth = 0;
        private float lastHeight = 0;
        
        private void Awake()
        {
            currentCamera = GetComponent<Camera>();
        }

        private void Update()
        {
            if (lastWidth == Screen.width && lastHeight == Screen.height)
                return;

            lastWidth = Screen.width;
            lastHeight = Screen.height;

            Rect rect = currentCamera.rect;

            float currentAspectRatio = lastWidth / lastHeight;
            float scaleheight = currentAspectRatio / ((float)ASPECT_RATIO_WIDTH / ASPECT_RATIO_HEIGHT); // (가로 / 세로)
            float scalewidth = 1f / scaleheight;
            if (scaleheight < 1)
            {
                rect.height = scaleheight;
                rect.y = (1f - scaleheight) / 2f;
            }
            else
            {
                rect.width = scalewidth;
                rect.x = (1f - scalewidth) / 2f;
            }

            currentCamera.rect = rect;
        }

        private void OnPreCull()
        {
            GL.Clear(true, true, Color.black);
        }
    }
}
