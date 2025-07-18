using UnityEngine;

namespace DadVSMe
{
    public class EntityMovement : MonoBehaviour
    {
        [SerializeField] Rigidbody2D entityRigidbody;
        private Vector2 movementVelocity = Vector2.zero;

        public Vector2 GetMovementVelocity()
        {
            return movementVelocity;
        }

        public void SetMovementVelocity(Vector2 velocity)
        {
            movementVelocity = velocity;
        }

        private void FixedUpdate()
        {
            entityRigidbody.linearVelocity = movementVelocity;
        }
    }
}
