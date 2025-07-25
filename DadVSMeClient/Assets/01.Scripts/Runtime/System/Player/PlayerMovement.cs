using UnityEngine;
using DadVSMe.Inputs;

namespace DadVSMe.Players
{
    [RequireComponent(typeof(EntityMovement))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] float moveSpeed = 5f;

        private EntityMovement entityMovement;

        private void Awake()
        {
            entityMovement = GetComponent<EntityMovement>();
        }

        private void Update()
        {
            PlayerInputReader inputReader = InputManager.GetInput<PlayerInputReader>();
            Vector2 movementInput = inputReader.MovementInput;
            if(inputReader.IsDashed)
                movementInput.x *= 3f;
            entityMovement.SetMovementVelocity(movementInput * moveSpeed);
        }
    }
}