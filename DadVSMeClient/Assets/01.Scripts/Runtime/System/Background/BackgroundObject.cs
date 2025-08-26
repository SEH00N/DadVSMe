using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe.Background
{
    public class BackgroundObject : MonoBehaviour, IPoolableBehaviour
    {
        private PoolReference _poolReference;
        public PoolReference PoolReference => _poolReference;

        [SerializeField] Transform _pivotTransform;

        [SerializeField] Transform _socketTransform;
        public Vector2 SocketPosition => _socketTransform.position;

        private int _themeIdx;
        public int ThemeIdx => _themeIdx;

        private void Awake()
        {
            _poolReference = GetComponent<PoolReference>();
        }

        public void Initialize(Vector2 spawnPosition, Transform followTransform, int themeIdx)
        {
            Vector2 delta = spawnPosition - (Vector2)_pivotTransform.position;
            transform.position = new Vector3
            (
                 transform.position.x + delta.x,
                 transform.position.y + delta.y,
                 transform.parent.position.z
            );

            _themeIdx = themeIdx;
        }

        public void OnSpawned()
        {
        }

        public void OnDespawn()
        {
        }
    }
}
