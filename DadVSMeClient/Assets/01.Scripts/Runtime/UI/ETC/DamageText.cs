using System.Threading;
using Cysharp.Threading.Tasks;
using H00N.Resources.Pools;
using ShibaInspector.Collections;
using TMPro;
using UnityEngine;

namespace DadVSMe
{
    public class DamageText : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private TextMeshPro tmp;      // UGUI 말고 TextMeshPro (MeshRenderer)
        [SerializeField] private Transform pivot;      // 이동/스케일 기준 (없으면 this)
        [SerializeField] PoolReference poolReference;

        [Header("Render (2D)")]
        [SerializeField] private string sortingLayerName = "UI"; // 2D용 소팅 레이어
        [SerializeField] private int sortingOrder = 100;         // 위에 보이도록
        [SerializeField] int fontSizeMin = 10;
        [SerializeField] int fontSizeMax = 10;
        
        [Header("Colors")]
        [SerializeField] Color defaultColor = Color.white;
        [SerializeField] Color criticalColor = Color.red;
        [SerializeField] SerializableDictionary<EAttackAttribute, Color> attackAttributeColors = new SerializableDictionary<EAttackAttribute, Color>();

        [Header("Anim")]
        [Min(0.1f)] public float duration = 0.6f;     // 전체 표시 시간
        public float riseWorld = 1.0f;                 // 위로 뜨는 월드 거리
        public AnimationCurve alphaCurve = new(
            new Keyframe(0f, 0f), new Keyframe(0.08f, 1f),
            new Keyframe(0.8f, 1f), new Keyframe(1f, 0f));
        public AnimationCurve scaleCurve = new(
            new Keyframe(0f, 0.9f), new Keyframe(0.12f, 1.25f), new Keyframe(1f, 1f));

        [Header("Spawn Jitter (world)")]
        public Vector2 jitterXY = new(0.15f, 0.10f);   // 살짝 흔들림

        // 내부 상태
        Camera _cam;                    // Orthographic 카메라
        Transform _follow;
        Vector3 _offset;
        Vector3 _spawnJitter;
        CancellationTokenSource _cts;
        Vector3 originPos;

#if UNITY_EDITOR
        void Reset()
        {
            if (!tmp) tmp = GetComponentInChildren<TextMeshPro>();
            if (!pivot) pivot = transform;
        }
#endif

        public void Setup(Camera orthoCamera, string sortingLayer = null, int? order = null)
        {
            _cam = orthoCamera ? orthoCamera : Camera.main;
            if (!tmp) tmp = GetComponentInChildren<TextMeshPro>();
            if (!pivot) pivot = transform;

            var r = tmp.GetComponent<Renderer>();
            if (!string.IsNullOrEmpty(sortingLayer)) sortingLayerName = sortingLayer;
            if (order.HasValue) sortingOrder = order.Value;
            r.sortingLayerName = sortingLayerName;
            r.sortingOrder = sortingOrder;

            // 2D 오쏘: 정면 고정(Z축 앞을 보게)
            pivot.rotation = Quaternion.identity;
        }

        /// <summary>데미지 텍스트 시작</summary>
        public void Play(Transform follow, Vector3 headOffsetWorld, int amount, bool critical, EAttackAttribute attackAttribute, float critScaleMul = 1.25f)
        {
            _follow = follow;
            _offset = headOffsetWorld;

            tmp.SetText(amount.ToString("N0"));
            tmp.color = critical ? criticalColor : GetColor(attackAttribute);
            tmp.fontSize = Random.Range(fontSizeMin, fontSizeMax);

            _spawnJitter = new Vector3(
                Random.Range(-jitterXY.x, jitterXY.x),
                Random.Range(0f, jitterXY.y),
                0f
            );

            originPos = _follow.position + _offset + _spawnJitter;
            pivot.position = originPos;
            SetAlpha(0f);
            pivot.localScale = Vector3.one; // 오쏘는 거리 무관 → 고정 스케일

            _cts?.Cancel();
            _cts?.Dispose();
            _cts = new CancellationTokenSource();
            AnimateAsync(critical ? critScaleMul : 1f, _cts.Token).Forget();
        }

        public void Stop()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
            SetAlpha(0f);
        }

        async UniTaskVoid AnimateAsync(float scaleMul, CancellationToken token)
        {
            float t = 0f;

            while (t < duration)
            {
                token.ThrowIfCancellationRequested();
                t += Time.deltaTime;
                float u = Mathf.Clamp01(t / duration);

                // 위치: 머리 기준 + 상승(오쏘라 거리 보정 불필요)
                Vector3 basePos = originPos + _offset + _spawnJitter;
                pivot.position = basePos + Vector3.up * (riseWorld * u);

                // 스케일/알파
                pivot.localScale = Vector3.one * (scaleCurve.Evaluate(u) * scaleMul);
                SetAlpha(alphaCurve.Evaluate(u));

                // 2D 빌보딩(필요 시만): 카메라가 회전한다면 정면 유지
                if (_cam && _cam.orthographic)
                    pivot.forward = Vector3.forward;

                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }

            SetAlpha(0f);

            PoolManager.Despawn(poolReference);
        }

        void SetAlpha(float a)
        {
            var c = tmp.color; c.a = a; tmp.color = c;
        }

        private Color GetColor(EAttackAttribute attackAttribute)
        {
            if(attackAttributeColors.TryGetValue(attackAttribute, out Color color))
                return color;

            return defaultColor;
        }
        void OnDisable() => Stop();
    }
}
