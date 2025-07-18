using UnityEngine;
using DadVSMe.NPCs;

namespace DadVSMe.Tests
{
    public class TestNPCMovement : MonoBehaviour
    {
        [SerializeField] NPCMovement npcMovement;

        private void Update()
        {
            if(Input.GetMouseButtonDown(0))
            {
                Vector2 mousePosition = Input.mousePosition;
                Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
                
                RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);
                if(hit == false)
                    return;

                npcMovement.SetDestination(hit.point);
            }
        }
    }
}