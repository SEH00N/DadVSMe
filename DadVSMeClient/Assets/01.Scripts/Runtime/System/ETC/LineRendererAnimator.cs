using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace DadVSMe
{
    [RequireComponent(typeof(LineRenderer))]
    public class LineRendererAnimator : MonoBehaviour
    {
        [Header("Durations")]
        private float appearDuration    = 2.5f;  // 등장(그려짐)
        private float disappearDuration = 2.5f;  // 소멸(앞에서부터 지워짐)
        [SerializeField] private bool  useUnscaledTime   = false;
        [SerializeField] private bool  loop              = false; // 등장 → 소멸을 반복

        private LineRenderer lineRenderer;
        private Vector3[] linePoints; // 원본 포인트(불변)
        private int pointsCount;

        private CancellationTokenSource _cts;

        private void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        private void OnDestroy()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }

        /// <summary>
        /// 등장→소멸을 한 세트로 실행.
        /// </summary>
        public void StartAnimation(float totalDuration)
        {
            // 절반씩 배분하고 싶을 때 편의용
            appearDuration    = Mathf.Max(0.0001f, totalDuration * 0.5f);
            disappearDuration = Mathf.Max(0.0001f, totalDuration * 0.5f);
            StartAnimation(appearDuration, disappearDuration);
        }

        /// <summary>
        /// 등장/소멸 각 지속 시간을 명시적으로 지정.
        /// </summary>
        public void StartAnimation(float appear, float disappear)
        {
            // 원본 포인트 캐싱
            pointsCount = lineRenderer.positionCount;
            if (pointsCount < 2) return;

            linePoints = new Vector3[pointsCount];
            for (int i = 0; i < pointsCount; i++)
                linePoints[i] = lineRenderer.GetPosition(i);

            appearDuration    = Mathf.Max(0.0001f, appear);
            disappearDuration = Mathf.Max(0.0001f, disappear);

            _cts?.Cancel();
            _cts?.Dispose();
            _cts = new CancellationTokenSource();
            RunAsync(_cts.Token).Forget();
        }

        private async UniTaskVoid RunAsync(CancellationToken token)
        {
            do
            {
                await AppearAsync(token);
                await DisappearAsync(token);
            }
            while (loop && !token.IsCancellationRequested);
        }

        // ------------------------------------------------------
        // 1) Appear: 시작점→끝점으로 "그려지듯" 등장
        //   - i번째 세그먼트를 진행하며 i+1..끝 포인트를 pos로 당김
        // ------------------------------------------------------
        private async UniTask AppearAsync(CancellationToken token)
        {
            if (pointsCount < 2 || appearDuration <= 0f) return;

            // 초기 상태: 모든 뒤 포인트를 시작점으로 겹치게
            lineRenderer.positionCount = pointsCount;
            for (int i = 0; i < pointsCount; i++)
                lineRenderer.SetPosition(i, linePoints[0]);

            float segmentDuration = appearDuration / (pointsCount - 1);

            for (int i = 0; i < pointsCount - 1; i++)
            {
                Vector3 start = linePoints[i];
                Vector3 end   = linePoints[i + 1];

                float elapsed = 0f;
                while (elapsed < segmentDuration)
                {
                    token.ThrowIfCancellationRequested();

                    elapsed += (useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime);
                    float t   = Mathf.Clamp01(elapsed / segmentDuration);
                    Vector3 pos = Vector3.Lerp(start, end, t);

                    // 0..i 까지는 원래 점 유지, i+1..끝은 pos로 당겨 "그려지는" 느낌
                    for (int k = 0; k <= i; k++)
                        lineRenderer.SetPosition(k, linePoints[k]);
                    for (int k = i + 1; k < pointsCount; k++)
                        lineRenderer.SetPosition(k, pos);

                    await UniTask.Yield(PlayerLoopTiming.Update, token);
                }

                // 세그먼트 종료 스냅(다음 세그먼트 시작 전 정확히 고정)
                for (int k = 0; k <= i + 1; k++)
                    lineRenderer.SetPosition(k, linePoints[k]);
                for (int k = i + 2; k < pointsCount; k++)
                    lineRenderer.SetPosition(k, linePoints[i + 1]);
            }

            // 최종적으로 전체 원본 포인트로 스냅
            for (int i = 0; i < pointsCount; i++)
                lineRenderer.SetPosition(i, linePoints[i]);
        }

        // ------------------------------------------------------
        // 2) Disappear: 시작점부터 "앞에서부터" 지워지듯 소멸
        //   - i번째 세그먼트를 진행하며 0..i 포인트를 pos로 밀어 넣음
        // ------------------------------------------------------
        private async UniTask DisappearAsync(CancellationToken token)
        {
            if (pointsCount < 2 || disappearDuration <= 0f) return;

            // 초기 상태: 전체가 다 보이는 상태에서 시작
            for (int i = 0; i < pointsCount; i++)
                lineRenderer.SetPosition(i, linePoints[i]);

            float segmentDuration = disappearDuration / (pointsCount - 1);

            for (int i = 0; i < pointsCount - 1; i++)
            {
                Vector3 start = linePoints[i];
                Vector3 end   = linePoints[i + 1];

                float elapsed = 0f;
                while (elapsed < segmentDuration)
                {
                    token.ThrowIfCancellationRequested();

                    elapsed += (useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime);
                    float t   = Mathf.Clamp01(elapsed / segmentDuration);
                    Vector3 pos = Vector3.Lerp(start, end, t);

                    // 0..i 를 pos로 "겹치게" 하여 앞부분이 사라지듯 보이게
                    for (int k = 0; k <= i; k++)
                        lineRenderer.SetPosition(k, pos);

                    // 나머지는 원래 위치 유지
                    for (int k = i + 1; k < pointsCount; k++)
                        lineRenderer.SetPosition(k, linePoints[k]);

                    await UniTask.Yield(PlayerLoopTiming.Update, token);
                }

                // 세그먼트 종료 스냅: 0..i+1 까지를 i+1번째 점으로 고정
                for (int k = 0; k <= i + 1; k++)
                    lineRenderer.SetPosition(k, linePoints[i + 1]);
                for (int k = i + 2; k < pointsCount; k++)
                    lineRenderer.SetPosition(k, linePoints[k]);
            }

            // 최종적으로 전부 "사라진" 상태(모두 마지막 점으로 겹치게)
            Vector3 last = linePoints[pointsCount - 1];
            for (int i = 0; i < pointsCount; i++)
                lineRenderer.SetPosition(i, last);
        }
    }
}
