using UnityEngine;

namespace DadVSMe.UI
{
    [ExecuteAlways]
    public class RectTransformSafeArea : MonoBehaviour
    {
        private RectTransform rectTransform;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            ApplySafeArea();
        }

        #if UNITY_EDITOR
        private Rect lastSafeArea;
        private Vector2Int lastScreenSize;
        private ScreenOrientation lastOrientation;
        private void Update()
        {
            bool screenSizeChanged = lastScreenSize.x != Screen.width || lastScreenSize.y != Screen.height;
            bool orientationChanged = lastOrientation != Screen.orientation;
            bool safeAreaChanged = lastSafeArea != Screen.safeArea;

            if (screenSizeChanged == false && orientationChanged == false && safeAreaChanged == false)
                return;

            ApplySafeArea();
            lastSafeArea = Screen.safeArea;
            lastScreenSize = new Vector2Int(Screen.width, Screen.height);
            lastOrientation = Screen.orientation;
        }
        #endif

        private void ApplySafeArea()
        {
            Rect safeArea = Screen.safeArea;
            Vector2Int screenSize = new Vector2Int(Screen.width, Screen.height);

            // 안전한 값 범위 보장
            Vector2 anchorMin = safeArea.position;
            Vector2 anchorMax = safeArea.position + safeArea.size;
            
            // 0~1 범위로 정규화
            anchorMin.x = Mathf.Clamp01(anchorMin.x / screenSize.x);
            anchorMin.y = Mathf.Clamp01(anchorMin.y / screenSize.y);
            anchorMax.x = Mathf.Clamp01(anchorMax.x / screenSize.x);
            anchorMax.y = Mathf.Clamp01(anchorMax.y / screenSize.y);

            // 값이 변경된 경우에만 적용
            if (rectTransform.anchorMin != anchorMin || rectTransform.anchorMax != anchorMax)
            {
                rectTransform.anchorMin = anchorMin;
                rectTransform.anchorMax = anchorMax;
            }
        }
    }
}
