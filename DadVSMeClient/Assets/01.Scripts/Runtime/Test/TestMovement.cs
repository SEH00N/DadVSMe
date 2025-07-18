using UnityEngine;
using DadVSMe.Inputs;

namespace DadVSMe.Tests
{
    public class TestMovement : MonoBehaviour
    {
        [SerializeField] float moveSpeed = 5f;
        [SerializeField] Rigidbody2D characterRigidbody = null;

        private Vector2 movementVelocity = Vector2.zero;

        private void Update()
        {
            PlayerInputReader inputReader = InputManager.GetInput<PlayerInputReader>();
            Vector2 movementInput = inputReader.MovementInput;
            movementVelocity = movementInput * moveSpeed;
        }

        private void FixedUpdate()
        {
            characterRigidbody.linearVelocity = movementVelocity;
        }
    }
}
