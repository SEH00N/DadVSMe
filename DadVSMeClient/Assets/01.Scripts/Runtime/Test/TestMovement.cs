using UnityEngine;
using DadVSMe.Inputs;

namespace DadVSMe
{
    public class TestMovement : MonoBehaviour
    {
        [SerializeField] float moveSpeed = 5f;
        [SerializeField] Rigidbody2D characterRigidbody = null;

        private void Update()
        {
            Vector2 movementInput = InputManager.GetInput<PlayerInputReader>().MovementInput;
            characterRigidbody.linearVelocity = movementInput * moveSpeed;
        }
    }
}
