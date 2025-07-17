using UnityEngine;

namespace DadVSMe.Entities
{
    public class EntityVisual : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer = null;
        // private bool isForwardRendered = false;
        private int triggerCount = 0;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            triggerCount++;
            if(triggerCount <= 0)
                return;

            // isForwardRendered = true;
            spriteRenderer.sortingOrder = 1;
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            triggerCount--;
            if(triggerCount > 0)
                return;

            // isForwardRendered = false;
            spriteRenderer.sortingOrder = 0;
        }
    }
}