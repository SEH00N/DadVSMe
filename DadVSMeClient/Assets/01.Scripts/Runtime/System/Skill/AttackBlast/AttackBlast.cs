using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using DadVSMe.Entities;
using H00N.AI.FSM;
using H00N.Resources.Addressables;
using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe
{
    public class AttackBlast : MonoBehaviour
    {
        [SerializeField]
        private AttackDataBase attackData;
        [SerializeField]
        private AddressableAsset<PoolableEffect> startEffectPrefab;
        [SerializeField]
        private Transform effectSpawnPoint;

        private UnitMovement movement;
        private PoolReference poolReference;

        private Unit instigator;
        private Vector3 originScale;

        [SerializeField]
        private float moveSpeed;
        private float lifeTime;

        private CancellationTokenSource _lifetimeCts;

        void Awake()
        {
            Initialize();
            originScale = transform.localScale;
        }

        void OnEnable()
        {
            transform.localScale = originScale;
        }

        public void Initialize()
        {
            movement = GetComponent<UnitMovement>();
            poolReference = GetComponent<PoolReference>();
        }

        public void SetInstigator(Unit instigator)
        {
            this.instigator = instigator;
        }

        public async void Launch(Vector3 direction, float lifeTime)
        {
            this.lifeTime = lifeTime;
            movement.SetActive(true);
            movement.SetMovementVelocity(direction * moveSpeed);

            _lifetimeCts?.Cancel();
            _lifetimeCts?.Dispose();
            _lifetimeCts = null;

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

        private async void PlayStartEffectAsync()
        {
            await startEffectPrefab.InitializeAsync();
            PoolableEffect effect = PoolManager.Spawn<PoolableEffect>(startEffectPrefab.Key);
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

            if (collision.gameObject.TryGetComponent<UnitHealth>(out UnitHealth targetHealth))
            {
                targetHealth.Attack(instigator, attackData);
                UnitFSMData unitFSMData = instigator.GetComponent<FSMBrain>().GetAIData<UnitFSMData>();
                _ = new PlayHitFeedback(attackData, unitFSMData.attackAttribute, targetHealth.transform.position, Vector3.zero, unitFSMData.forwardDirection);

            }

            // _lifetimeCts?.Cancel();

            // PoolManager.Despawn(poolReference);
        }
    }
}
