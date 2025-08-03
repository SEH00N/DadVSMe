using UnityEngine;

namespace DadVSMe
{
    public class EntityMovement : MonoBehaviour
    {
        [SerializeField] Rigidbody2D entityRigidbody;

        private bool isActive = false;
        public bool IsActive => isActive;

        private Vector2 movementVelocity = Vector2.zero;
        public Vector2 MovementVelocity => movementVelocity;

        public void SetActive(bool isActive)
        {
            if(this.isActive == isActive)
                return;

            this.isActive = isActive;

            // if(isActive == false)
            // {
                if(movementVelocity != Vector2.zero)
                {
                    movementVelocity = Vector2.zero;
                    entityRigidbody.linearVelocity = Vector2.zero;
                }
            // }
        }

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
            if(isActive == false)
                return;

            entityRigidbody.linearVelocity = movementVelocity;
        }
    }
}
