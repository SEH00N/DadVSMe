using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using DadVSMe.Entities;
using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe
{
    public class AttackBlast : MonoBehaviour
    {
        [SerializeField]
        private AttackDataBase attackData;

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

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject == instigator)
                return;
            if (collision.gameObject.CompareTag("Enemy") == false)
                return;

            if (collision.gameObject.TryGetComponent<UnitHealth>(out UnitHealth targetHealth))
            {
                targetHealth.Attack(instigator, attackData);
            }

            // _lifetimeCts?.Cancel();

            // PoolManager.Despawn(poolReference);
        }
    }
}
