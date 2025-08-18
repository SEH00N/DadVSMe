using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe.Animals
{
    public class SimpleProjectile : Projectile
    {
        [SerializeField] float lifeTime = 0f;
        [SerializeField] float moveSpeed = 0f;

        private CancellationTokenSource cancellationTokenSource = null;

        public override void Initialize(Vector3 targetPosition)
        {
            base.Initialize(targetPosition);

            Vector2 direction = (targetPosition - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);

            DespawnAfterLifeTimeAsync().Forget();
        }

        private void FixedUpdate()
        {
            transform.Translate(Vector3.forward * (moveSpeed * Time.fixedDeltaTime));
        }

        private async UniTask DespawnAfterLifeTimeAsync()
        {
            try {
                cancellationTokenSource?.Cancel();
                cancellationTokenSource?.Dispose();
                cancellationTokenSource = new CancellationTokenSource();

                await UniTask.Delay(TimeSpan.FromSeconds(lifeTime), cancellationToken: cancellationTokenSource.Token);

                cancellationTokenSource?.Dispose();
                cancellationTokenSource = null;

                PoolManager.Despawn(this);
            }
            catch(OperationCanceledException) { }
            finally {
                cancellationTokenSource?.Dispose();
                cancellationTokenSource = null;
            }
        }
    }
}