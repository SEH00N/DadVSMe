using UnityEngine;
using DadVSMe.Inputs;

namespace DadVSMe
{
    public class TestMovement : MonoBehaviour
    {
        [SerializeField] float moveSpeed = 5f;

        private void Update()
        {
            Vector2 movementInput = InputManager.GetInput<PlayerInputReader>().MovementInput;
            transform.Translate(movementInput * (moveSpeed * Time.deltaTime));
        }
    }
}
