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
            PlayerInputReader inputReader = InputManager.GetInput<PlayerInputReader>();
            Vector2 movementInput = inputReader.MovementInput;
            characterRigidbody.linearVelocity = movementInput * moveSpeed;

            if(inputReader.GetAttack1Down())
            {
                Debug.Log($"[TestMovement] Attack1 Down");
            }
            if(inputReader.GetAttack1Up())
            {
                Debug.Log($"[TestMovement] Attack1 Up");
            }
            if(inputReader.GetAttack1Press())
            {
                Debug.Log($"[TestMovement] Attack1 Press");
            }

            if(inputReader.GetAttack2Down())
            {
                Debug.Log($"[TestMovement] Attack2 Down");
            }
            if(inputReader.GetAttack2Up())
            {
                Debug.Log($"[TestMovement] Attack2 Up");
            }
            if(inputReader.GetAttack2Press())
            {
                Debug.Log($"[TestMovement] Attack2 Press");
            }
        }
    }
}
