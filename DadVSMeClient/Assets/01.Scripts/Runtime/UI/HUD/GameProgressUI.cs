using UnityEngine;

namespace DadVSMe.UI.HUD
{
    public class GameProgressUI : MonoBehaviourUI
    {
        private const float UPDATE_INTERVAL = 0.1f;

        [SerializeField] RectTransform deadLineTransform = null;
        [SerializeField] RectTransform playerTransform = null;

        private Transform startLine = null;
        private Transform endLine = null;
        private Transform deadLine = null;
        private Transform player = null;

        private bool initialized = false;
        private float timer = 0f;

        public void Initialize(Transform startLine, Transform endLine, Transform deadLine, Transform player)
        {
            this.startLine = startLine;
            this.endLine = endLine;
            this.deadLine = deadLine;
            this.player = player;

            initialized = true;
        }

        private void Update()
        {
            if(initialized == false)
                return;

            timer += Time.deltaTime;
            if(timer < UPDATE_INTERVAL)
                return;

            timer = 0f;
            UpdateTransform(deadLineTransform, deadLine);
            UpdateTransform(playerTransform, player);
        }

        private void UpdateTransform(RectTransform uiTransform, Transform target)
        {
            float progress = Mathf.Clamp01(target.position.x / (endLine.position.x - startLine.position.x));

            uiTransform.anchorMin = new Vector2(progress, uiTransform.anchorMin.y);
            uiTransform.anchorMax = new Vector2(progress, uiTransform.anchorMax.y);
            uiTransform.anchoredPosition = new Vector2(0f, uiTransform.anchoredPosition.y);
        }
    }
}
