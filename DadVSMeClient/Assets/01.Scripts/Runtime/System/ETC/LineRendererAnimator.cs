using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace DadVSMe
{
    [RequireComponent(typeof(LineRenderer))]
    public class LineRendererAnimator : MonoBehaviour
    {
        [Header("Per-Segment Durations (seconds)")]
        [SerializeField] private float appearSegmentDuration    = 0.15f; // n~n+1 그리기 시간
        [SerializeField] private float disappearSegmentDuration = 0.15f; // n~n+1 지우기 시간
        [SerializeField] private bool  useUnscaledTime          = false;
        [SerializeField] private bool  loop                     = false; // appear → disappear 반복

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
        /// 세그먼트당 시간으로 시작(등장/소멸 각각).
        /// </summary>
        public void StartAnimationPerSegment(float appearPerSeg, float disappearPerSeg)
        {
            appearSegmentDuration    = Mathf.Max(0.0001f, appearPerSeg);
            disappearSegmentDuration = Mathf.Max(0.0001f, disappearPerSeg);
            CachePointsAndRun();
        }

        /// <summary>
        /// 총 시간을 반영: (총 시간) / (세그먼트 수) 로 세그먼트 시간을 자동 계산.
        /// </summary>
        public void StartAnimationTotal(float totalAppear, float totalDisappear)
        {
            // 포인트 캐시가 없으면 일단 캐싱
            if (!TryCachePointsOnce())
                return;

            int segCount = Mathf.Max(1, pointsCount - 1);
            appearSegmentDuration    = Mathf.Max(0.0001f, totalAppear    / segCount);
            disappearSegmentDuration = Mathf.Max(0.0001f, totalDisappear / segCount);

            Run();
        }

        /// <summary>
        /// 기존처럼 하나의 totalDuration만 주면 1:1로 나눠 쓰는 편의 함수.
        /// </summary>
        public void StartAnimationTotal(float totalDuration)
        {
            if (!TryCachePointsOnce())
                return;

            int segCount = Mathf.Max(1, pointsCount - 1);
            float half = Mathf.Max(0.0001f, totalDuration * 0.5f);
            appearSegmentDuration    = half / segCount;
            disappearSegmentDuration = half / segCount;

            Run();
        }

        // 내부: 포인트 캐싱 + 실행
        void CachePointsAndRun()
        {
            if (!TryCachePointsOnce()) return;
            Run();
        }

        bool TryCachePointsOnce()
        {
            pointsCount = lineRenderer.positionCount;
            if (pointsCount < 2) return false;

            linePoints = new Vector3[pointsCount];
            for (int i = 0; i < pointsCount; i++)
                linePoints[i] = lineRenderer.GetPosition(i);
            return true;
        }

        void Run()
        {
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
        // Appear: 시작점→끝점으로 "그려지듯" 등장
        //   - 세그먼트당 시간: appearSegmentDuration
        // ------------------------------------------------------
        private async UniTask AppearAsync(CancellationToken token)
        {
            if (pointsCount < 2 || appearSegmentDuration <= 0f) return;

            // 초기: 모두 시작점으로 겹치게
            lineRenderer.positionCount = pointsCount;
            for (int i = 0; i < pointsCount; i++)
                lineRenderer.SetPosition(i, linePoints[0]);

            for (int i = 0; i < pointsCount - 1; i++)
            {
                Vector3 start = linePoints[i];
                Vector3 end   = linePoints[i + 1];

                float elapsed = 0f;
                while (elapsed < appearSegmentDuration)
                {
                    token.ThrowIfCancellationRequested();

                    elapsed += (useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime);
                    float t   = Mathf.Clamp01(elapsed / appearSegmentDuration);
                    Vector3 pos = Vector3.Lerp(start, end, t);

                    // 0..i 원본 고정, i+1..끝은 pos로 당겨서 "자라나는" 효과
                    for (int k = 0; k <= i; k++)
                        lineRenderer.SetPosition(k, linePoints[k]);
                    for (int k = i + 1; k < pointsCount; k++)
                        lineRenderer.SetPosition(k, pos);

                    await UniTask.Yield(PlayerLoopTiming.Update, token);
                }

                // 세그먼트 종료 스냅
                for (int k = 0; k <= i + 1; k++)
                    lineRenderer.SetPosition(k, linePoints[k]);
                for (int k = i + 2; k < pointsCount; k++)
                    lineRenderer.SetPosition(k, linePoints[i + 1]);
            }

            // 최종: 전체 원본으로 정렬
            for (int i = 0; i < pointsCount; i++)
                lineRenderer.SetPosition(i, linePoints[i]);
        }

        // ------------------------------------------------------
        // Disappear: 시작점부터 "앞에서부터" 지워지듯 소멸
        //   - 세그먼트당 시간: disappearSegmentDuration
        // ------------------------------------------------------
        private async UniTask DisappearAsync(CancellationToken token)
        {
            if (pointsCount < 2 || disappearSegmentDuration <= 0f) return;

            // 시작: 전부 보이는 상태
            for (int i = 0; i < pointsCount; i++)
                lineRenderer.SetPosition(i, linePoints[i]);

            for (int i = 0; i < pointsCount - 1; i++)
            {
                Vector3 start = linePoints[i];
                Vector3 end   = linePoints[i + 1];

                float elapsed = 0f;
                while (elapsed < disappearSegmentDuration)
                {
                    token.ThrowIfCancellationRequested();

                    elapsed += (useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime);
                    float t   = Mathf.Clamp01(elapsed / disappearSegmentDuration);
                    Vector3 pos = Vector3.Lerp(start, end, t);

                    // 0..i 를 pos로 겹치게 하여 앞부분이 사라지듯
                    for (int k = 0; k <= i; k++)
                        lineRenderer.SetPosition(k, pos);

                    // 나머지는 원래 위치 유지
                    for (int k = i + 1; k < pointsCount; k++)
                        lineRenderer.SetPosition(k, linePoints[k]);

                    await UniTask.Yield(PlayerLoopTiming.Update, token);
                }

                // 세그먼트 종료 스냅: 0..i+1를 i+1번째 점으로 정렬
                for (int k = 0; k <= i + 1; k++)
                    lineRenderer.SetPosition(k, linePoints[i + 1]);
                for (int k = i + 2; k < pointsCount; k++)
                    lineRenderer.SetPosition(k, linePoints[k]);
            }

            // 최종: 모두 끝점으로 겹침(완전히 사라진 상태)
            Vector3 last = linePoints[pointsCount - 1];
            for (int i = 0; i < pointsCount; i++)
                lineRenderer.SetPosition(i, last);
        }
    }
}
