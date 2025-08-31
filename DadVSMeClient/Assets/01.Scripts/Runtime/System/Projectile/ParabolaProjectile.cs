using System.Collections;
using DadVSMe.Entities;
using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ParabolaProjectile : Projectile
    {
        private const float MIN_TIME_TO_TARGET = 1f;

        [Header("SimpleProjectile")]
        [SerializeField] string targetTag = "Enemy";
        [SerializeField] SimpleAttackData attackData = null;

        [Space(10f)]
        [SerializeField] float lifeTime = 0f;
        [SerializeField] float speed = 1f;
        // [SerializeField] float jumpDistance = 1f;
        private Rigidbody2D projectileRigidbody = null;

        protected override void Awake()
        {
            base.Awake();
            projectileRigidbody = GetComponent<Rigidbody2D>();
        }

        private void LateUpdate()
        {
            float angle = Mathf.Atan2(projectileRigidbody.linearVelocity.y, projectileRigidbody.linearVelocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
        }

        public override void Initialize(Unit owner, Vector2 targetPosition)
        {
            base.Initialize(owner, targetPosition);

            // float gravity = Physics2D.gravity.y * projectileRigidbody.gravityScale;

            // Vector2 startPosition = transform.position;
            // Vector2 displacement = targetPosition - startPosition;

            // float displacementX = displacement.x;

            // float apexHeight = Mathf.Max(startPosition.y, targetPosition.y) + jumpDistance;

            // float launchVelocityY = Mathf.Sqrt(-2 * gravity * (apexHeight - startPosition.y));
            // float timeToApex = launchVelocityY / -gravity;

            // float timeFromApexToTarget = Mathf.Sqrt(2 * (targetPosition.y - apexHeight) / gravity);
            // float totalTime = timeToApex + timeFromApexToTarget;
            // float launchVelocityX = displacementX / totalTime;

            // projectileRigidbody.linearVelocity = new Vector2(launchVelocityX, launchVelocityY);


            Vector2 startPosition = transform.position;
            Vector2 displacement = targetPosition - startPosition;

            float displacementX = displacement.x;
            float displacementY = displacement.y;

            float timeToTarget = Mathf.Max(MIN_TIME_TO_TARGET, Mathf.Abs(displacementX) / speed);
            float gravity = Physics2D.gravity.y * projectileRigidbody.gravityScale;
            float launchVelocityX = displacementX / timeToTarget;
            float launchVelocityY = (displacementY - 0.5f * gravity * (timeToTarget * timeToTarget)) / timeToTarget;

            projectileRigidbody.linearVelocity = new Vector2(launchVelocityX, launchVelocityY);

            StopAllCoroutines();
            StartCoroutine(DespawnCoroutine(lifeTime));
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.CompareTag(targetTag) == false)
                return;

            if(other.TryGetComponent<IHealth>(out IHealth unitHealth) == false)
                return;

            unitHealth.Attack(owner, attackData);

            UnitFSMData unitFSMData = owner.FSMBrain.GetAIData<UnitFSMData>();
            EAttackAttribute attackAttribute = unitFSMData.attackAttribute;
            unitFSMData.attackAttribute = EAttackAttribute.Normal;

            _ = new PlayHitFeedback(attackData, unitFSMData.attackAttribute, transform.position, Vector3.zero, unitFSMData.forwardDirection);

            unitFSMData.attackAttribute = attackAttribute;

            StopAllCoroutines();
            PoolManager.Despawn(this);
        }

        private IEnumerator DespawnCoroutine(float lifeTime)
        {
            yield return new WaitForSeconds(lifeTime);
            PoolManager.Despawn(this);
        }
    }
}
