using UnityEngine;

namespace DadVSMe
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class GravityBaseProjectile : Projectile
    {
        [SerializeField] float speed = 1f;
        [SerializeField] float jumpDistance = 1f;
        private Rigidbody2D projectileRigidbody = null;

        protected override void Awake()
        {
            base.Awake();
            projectileRigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (projectileRigidbody.linearVelocity.sqrMagnitude <= 0.01f)
                return;

            UpdateRotation();
        }

        public override void Initialize(Vector2 targetPosition)
        {
            base.Initialize(targetPosition);

            float gravity = Physics2D.gravity.y * projectileRigidbody.gravityScale;

            Vector2 startPosition = transform.position;
            Vector2 displacement = targetPosition - startPosition;

            float displacementX = displacement.x;
            float displacementY = displacement.y;

            // float apexHeight = Mathf.Max(startPosition.y, targetPosition.y) + Mathf.Abs(displacementY) * jumpDistance;
            float apexHeight = Mathf.Max(startPosition.y, targetPosition.y) + jumpDistance * speed;

            float launchVelocityY = Mathf.Sqrt(-2 * gravity * (apexHeight - startPosition.y));
            float timeToApex = launchVelocityY / -gravity;

            float timeFromApexToTarget = Mathf.Sqrt(2 * (targetPosition.y - apexHeight) / gravity);
            float totalTime = timeToApex + timeFromApexToTarget;
            float launchVelocityX = displacementX / totalTime;

            projectileRigidbody.gravityScale = speed * speed;
            projectileRigidbody.linearVelocity = new Vector2(launchVelocityX, launchVelocityY) * speed;
        }

        private void UpdateRotation()
        {
            float angle = Mathf.Atan2(projectileRigidbody.linearVelocity.y, projectileRigidbody.linearVelocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle + 90f);
        }
    }
}
