using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DadVSMe.Entities;
using DG.Tweening;
using H00N.AI.FSM;
using H00N.Resources.Addressables;
using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe
{
    public class AttackBlast : MonoBehaviour
    {
        private const float ANGLE_RANDOMNESS = 5f;

        [SerializeField]
        private AttackDataBase attackData;
        [SerializeField]
        private AddressableAsset<PoolableEffect> startEffectPrefab;
        [SerializeField]
        private Transform effectSpawnPoint;
        [SerializeField]
        private SpriteRenderer visualRenderer = null;

        private UnitMovement movement;
        private PoolReference poolReference;

        private Unit instigator;
        private Vector3 originScale;

        [SerializeField] float moveSpeed;

        private static bool timeScaleEffectAvailable = true;
        private CancellationTokenSource _lifetimeCts;

        private int additiveDamage;

        void Awake()
        {
            movement = GetComponent<UnitMovement>();
            poolReference = GetComponent<PoolReference>();
            originScale = transform.localScale;
        }

        void OnEnable()
        {
            transform.localScale = originScale;
        }

        public void Initialize(int additiveDamage)
        {
            this.additiveDamage = additiveDamage;
            _ = new InitializeAttackFeedback(attackData);
        }

        public void SetInstigator(Unit instigator)
        {
            this.instigator = instigator;
        }

        public async void Launch(Vector3 direction, float lifeTime)
        {
            float angle = Random.Range(-ANGLE_RANDOMNESS, ANGLE_RANDOMNESS);
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            direction = (rotation * direction).normalized;
            transform.rotation = rotation;

            movement.SetActive(true);
            movement.SetMovementVelocity(direction * moveSpeed);

            _lifetimeCts?.Cancel();
            _lifetimeCts?.Dispose();
            _lifetimeCts = null;

            PlayTween(lifeTime);
            PlayStartEffectAsync();

            _lifetimeCts = new CancellationTokenSource();
            var token = _lifetimeCts.Token;

            await UniTask
                .Delay(System.TimeSpan.FromSeconds(lifeTime), cancellationToken: token)
                .SuppressCancellationThrow();

            // 취소되었다면(트리거로) 중복 Despawn 방지
            if (!token.IsCancellationRequested)
            {
                PoolManager.Despawn(poolReference);
            }
        }

        private async void PlayTween(float lifeTime)
        {
            visualRenderer.color = new Color(visualRenderer.color.r, visualRenderer.color.g, visualRenderer.color.b, 0);
            await visualRenderer.DOFade(1, lifeTime * 0.4f).SetEase(Ease.OutSine);
            await UniTask.Delay(System.TimeSpan.FromSeconds(lifeTime * 0.2f));
            await visualRenderer.DOFade(0, lifeTime * 0.4f).SetEase(Ease.InSine);
        }

        private async void PlayStartEffectAsync()
        {
            await startEffectPrefab.InitializeAsync();
            PoolableEffect effect = PoolManager.Spawn<PoolableEffect>(startEffectPrefab.Key, GameInstance.GameCycle.transform);
            effect.transform.position = effectSpawnPoint.position;
            effect.transform.localScale = new Vector3(Mathf.Abs(effect.transform.localScale.x) * Mathf.Sign(transform.localScale.x), effect.transform.localScale.y, effect.transform.localScale.z);
            effect.Play();
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject == instigator)
                return;
            if (collision.gameObject.CompareTag("Enemy") == false)
                return;

            if (collision.gameObject.TryGetComponent<IHealth>(out IHealth targetHealth))
            {
                PlayTimeScaleEffect();

                DynamicAttackData dynamicAttackData = new DynamicAttackData(attackData);
                dynamicAttackData.SetDamage(dynamicAttackData.Damage + additiveDamage);
                targetHealth.Attack(instigator, dynamicAttackData);
                
                UnitFSMData unitFSMData = instigator.GetComponent<FSMBrain>().GetAIData<UnitFSMData>();
                _ = new PlayHitFeedback(dynamicAttackData, unitFSMData.attackAttribute, targetHealth.Position, Vector3.zero, unitFSMData.forwardDirection);
            }

            // _lifetimeCts?.Cancel();

            // PoolManager.Despawn(poolReference);
        }

        private async void PlayTimeScaleEffect()
        {
            // Temp
            return;

            if(Time.timeScale != GameDefine.DEFAULT_TIME_SCALE)
                return;

            if(!timeScaleEffectAvailable)
                return;

            timeScaleEffectAvailable = false;

            TimeManager.SetTimeScale(0.3f, true);
            await UniTask.WaitForSeconds(0.05f);
            TimeManager.SetTimeScale(GameDefine.DEFAULT_TIME_SCALE, true);

            await UniTask.WaitForSeconds(2f);
            timeScaleEffectAvailable = true;
        }
    }
}
