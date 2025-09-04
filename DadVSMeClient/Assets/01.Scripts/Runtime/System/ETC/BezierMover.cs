using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace DadVSMe
{
    [DisallowMultipleComponent]
    public class BezierMover : MonoBehaviour
    {
        [Header("References")]
        private Transform target;           // 플레이어
        private Rigidbody rb;               // 선택

        [Header("Timing")]
        public float delayBeforeHoming = 0.15f;
        public float travelDuration = 0.6f;
        public AnimationCurve speedOverLife = AnimationCurve.EaseInOut(0, 0, 1, 1);

        [Header("Curve Shape")]
        public float controlRadiusMin = 0.6f;
        public float controlRadiusMax = 1.8f;
        public float upwardBias = 0.4f;
        public float sideRadius = 1f;

        [Header("Behavior")]
        public bool updateEndPointContinuously = true;
        public float collectDistance = 0.6f;
        public float maxExtraDuration = 0.3f;

        [Header("Gizmos")]
        public bool drawGizmos = false;

        public UnityEvent<Transform> onArrivedEvent;

        // 내부
        Vector3 p0, p1, p2, p3;
        CancellationTokenSource _cts;

        UniTask runningTask;
        private bool isRunning;
        public bool IsRunning => isRunning;

        public Vector3 Forward { get; private set; }
        public Vector2 Forward2D { get; private set; }

        void Reset()
        {
            rb = GetComponent<Rigidbody>();
        }

        void OnEnable()
        {
            _cts = new CancellationTokenSource();
        }

        void OnDisable()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }

        /// <summary>
        /// UniTask로 흡입 이동 시작. 외부 취소 토큰을 넘기면 연동됨.
        /// </summary>
        public UniTask LaunchAsync(Transform targetTransform, CancellationToken external = default)
        {
            if (isRunning)
                return runningTask;

            isRunning = true;
            target = targetTransform;
            // 내부/외부 토큰 연결
            var linked = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token, external);
            var ct = linked.Token;
            // 메인 태스크 시작
            runningTask = MoveRoutine(ct).ContinueWith(() =>
            {
                linked.Dispose();
            });

            return runningTask;
        }

        async UniTask MoveRoutine(CancellationToken ct)
        {
            p0 = (Vector2)transform.position;
            p3 = target ? (Vector2)target.position : p0 + Vector3.up * 0.1f;

            float dist = Vector2.Distance(p0, p3);

            // 2D 방향 벡터 계산
            var dir = (p3 - p0).sqrMagnitude > 0.0001f ? (p3 - p0).normalized : Vector3.right;
            // 2D right 벡터 계산 (90도 회전)
            var right = new Vector3(-dir.y, dir.x, 0);

            float r1 = dist * UnityEngine.Random.Range(0.4f, 0.8f);
            float r2 = dist * UnityEngine.Random.Range(0.4f, 0.8f);
            float side = UnityEngine.Random.value < 0.5f ? -sideRadius : sideRadius;

            // 2D 제어점 계산
            p1 = p0 
                + dir * dist * UnityEngine.Random.Range(0.4f, 0.7f)    // 진행 방향
                + right * side * r1;

            p2 = Vector3.Lerp(p0, p3, UnityEngine.Random.Range(0.45f, 0.8f))
                + right * -side * r2;

            // 물리 끄기(선택)
            if (rb) { rb.isKinematic = true; rb.linearVelocity = Vector3.zero; }

            Forward = dir;
            Forward2D = dir;

            // 짧은 딜레이 + 가벼운 떨림
            float delay = Mathf.Max(0f, delayBeforeHoming);
            if (delay > 0f)
            {
                Vector2 basePos = (Vector2)transform.position;
                float end = Time.time + delay;
                while (Time.time < end)
                {
                    ct.ThrowIfCancellationRequested();
                    float wobble = Mathf.Sin(Time.time * 60f) * 0.005f;
                    // Vector3 prevPosition = transform.position;
                    Vector3 targetPosition = basePos + (Random.insideUnitCircle * 0.02f) + (Vector2.up * wobble);
                    transform.position = targetPosition;

                    if(target)
                    {
                        Forward = (target.position - transform.position).normalized;
                        Forward2D = ((Vector2)target.position - (Vector2)transform.position).normalized;
                    }
                    await UniTask.Yield(PlayerLoopTiming.Update, ct);
                }
            }

            // 본 이동
            float duration = Mathf.Max(0.01f, travelDuration);
            float start = Time.time;
            while (Time.time - start < duration + maxExtraDuration)
            {
                ct.ThrowIfCancellationRequested();

                if (target)
                {
                    if (updateEndPointContinuously) 
                        p3 = (Vector2)target.position;

                    if ((transform.position - p3).sqrMagnitude <= collectDistance * collectDistance)
                    {
                        Arrive();
                        return;
                    }
                }

                float rawT = Mathf.Clamp01((Time.time - start) / duration);
                float eased = speedOverLife != null ? speedOverLife.Evaluate(rawT) : BezierUtility.EaseInOut(rawT);

                Vector3 prevPosition = transform.position;
                Vector3 targetPosition = BezierUtility.Cubic(p0, p1, p2, p3, eased);
                Forward = (targetPosition - prevPosition).normalized;
                Forward2D = ((Vector2)targetPosition - (Vector2)prevPosition).normalized;
                transform.position = targetPosition;

                await UniTask.Yield(PlayerLoopTiming.Update, ct);
            }

            // 안전 보정
            Arrive();
        }

        void Arrive()
        {
            onArrivedEvent?.Invoke(target);
            target = null;
            isRunning = false;
        }

        // 디버그 기즈모
        void OnDrawGizmos()
        {
            //if (!drawGizmos) return;

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(p0, .05f);
            Gizmos.DrawWireSphere(p1, .05f);
            Gizmos.DrawWireSphere(p2, .05f);
            Gizmos.DrawWireSphere(p3, .05f);

            Vector3 prev = p0;
            for (int i = 1; i <= 20; i++)
            {
                float tt = i / 20f;
                Vector3 point = BezierUtility.Cubic(p0, p1, p2, p3, tt);
                Gizmos.DrawLine(prev, point);
                prev = point;
            }

            // forward axis
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + (Vector3)Forward2D * 2.5f);

            // right axis
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.Cross(-Vector3.forward, (Vector3)Forward2D) * 2.5f); 
        }
    }
}

