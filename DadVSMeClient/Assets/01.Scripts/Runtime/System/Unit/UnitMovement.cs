using UnityEngine;

namespace DadVSMe
{
    public class UnitMovement : MonoBehaviour
    {
        [SerializeField] Rigidbody2D unitRigidbody;

        private bool isActive = false;
        public bool IsActive => isActive;

        private Vector2 movementVelocity = Vector2.zero;
        public Vector2 MovementVelocity => movementVelocity;

        // public void Initialize(UnitStat moveSpeedStat)
        // {
        //     moveSpeedStat.onStatChanged.AddListener(OnMoveSpeedStatChanged);
        // }

        public void SetActive(bool isActive)
        {
            this.isActive = isActive;
            if (movementVelocity != Vector2.zero)
            {
                movementVelocity = Vector2.zero;
                unitRigidbody.linearVelocity = Vector2.zero;
            }
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
            if (isActive == false)
                return;

            unitRigidbody.linearVelocity = movementVelocity;
        }
    }
}
