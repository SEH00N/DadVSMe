using UnityEngine;

namespace DadVSMe.Background
{
    public class BackgroundObject : MonoBehaviour
    {
        [SerializeField] Transform _pivotTransform;

        [SerializeField] Transform _socketTransform;
        public Vector2 SocketPosition => _socketTransform.position;

        public void Initialize(Vector2 spawnPosition)
        {
            Vector2 delta = spawnPosition - (Vector2)_pivotTransform.position;

            transform.position = new Vector3
            (
                 transform.position.x + delta.x,
                 transform.position.y + delta.y,
                 transform.position.z
             );
        }
    }
}
