using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe
{
    public class NinjaAfterImage : MonoBehaviour, IPoolableBehaviour
    {
        private PoolReference _poolReference;
        public PoolReference PoolReference => _poolReference;

        [SerializeField] SpriteRenderer _spriteRenderer;

        [SerializeField] float fadingTime = 0.2f;

        private void Awake()
        {
            _poolReference = GetComponent<PoolReference>();
        }

        public void Initialize(Vector2 spawnPosition, int forward)
        {
            transform.position = spawnPosition;

            bool isFlip = forward == 1 ? false : true;
            _spriteRenderer.flipX = isFlip;

            _spriteRenderer.color = Color.white;
        }

        public void OnEndAnimation()
        {
            PoolManager.Despawn(this);
        }

        public void OnSpawned()
        {
        }

        public void OnDespawn()
        {
        }
    }
}
