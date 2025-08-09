using UnityEngine;
using DadVSMe.Inputs;

namespace DadVSMe.Players
{
    [RequireComponent(typeof(UnitMovement))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] float moveSpeed = 5f;

        private UnitMovement unitMovement;

        private void Awake()
        {
            unitMovement = GetComponent<UnitMovement>();
        }

        private void Update()
        {
            PlayerInputReader inputReader = InputManager.GetInput<PlayerInputReader>();
            Vector2 movementInput = inputReader.MovementInput;
            if(inputReader.IsDashed)
                movementInput.x *= 3f;
            unitMovement.SetMovementVelocity(movementInput * moveSpeed);
        }
    }
}