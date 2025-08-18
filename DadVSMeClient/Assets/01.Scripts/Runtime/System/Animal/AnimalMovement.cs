using UnityEngine;

namespace DadVSMe.Animals
{
    public class AnimalMovement : MonoBehaviour
    {
        [SerializeField] float moveSpeed = 10f;
        private Transform followTarget = null;

        public void SetFollowTarget(Transform followTarget)
        {
            this.followTarget = followTarget;
        }

        private void FixedUpdate()
        {
            Vector3 direction = followTarget.position - transform.position;
            float distance = direction.magnitude;
            if(distance < 0.01f)
                return;

            float adjustedSpeed = Mathf.Min(moveSpeed, distance / Time.fixedDeltaTime);
            transform.Translate(direction.normalized * (adjustedSpeed * Time.fixedDeltaTime));
            transform.localScale = new Vector3(Mathf.Sign(direction.x), 1, 1);
        }
    }
}