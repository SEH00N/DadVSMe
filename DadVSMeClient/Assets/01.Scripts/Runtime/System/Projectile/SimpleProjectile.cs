using System.Collections;
using DadVSMe.Entities;
using H00N.Resources.Pools;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DadVSMe
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class SimpleProjectile : Projectile
    {
        private enum ESimpleProjectileCollisionType
        {
            None,
            Despawm,
            Collision
        }

        private const float BOUNCE_LIFE_TIME = 0.35f;
        private const float BOUNCE_GRAVITY_SCALE = 3f;
        private static readonly Vector2 BOUNCE_FORCE_RANDOMNESS = new Vector2(1f, 3f);
        private static readonly Vector2 BOUNCE_FORCE = new Vector2(4.5f, 5f);

        [Header("SimpleProjectile")]
        [SerializeField] string targetTag = "Enemy";
        [SerializeField] SimpleAttackData attackData = null;

        [Space(10f)]
        [SerializeField] float lifeTime = 0f;
        [SerializeField] ESimpleProjectileCollisionType collisionType = ESimpleProjectileCollisionType.None;
        [SerializeField] Vector2 initialVelocity = Vector2.zero;

        private Rigidbody2D projectileRigidbody = null;

        private bool isReleasing = false;
        private float defaultGravityScale = 0f;

        protected override void Awake()
        {
            base.Awake();
            projectileRigidbody = GetComponent<Rigidbody2D>();
            defaultGravityScale = projectileRigidbody.gravityScale;
        }

        public override void Initialize(Unit owner, Vector2 targetPosition)
        {
            base.Initialize(owner, targetPosition);

            isReleasing = false;

            float directionX = targetPosition.x - transform.position.x;
            projectileRigidbody.linearVelocity = new Vector2(directionX * initialVelocity.x, initialVelocity.y);
            projectileRigidbody.gravityScale = defaultGravityScale;

            StopAllCoroutines();
            StartCoroutine(DespawnCoroutine(lifeTime));
        }

        private void LateUpdate()
        {
            float angle = Mathf.Atan2(projectileRigidbody.linearVelocity.y, projectileRigidbody.linearVelocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(isReleasing)
                return;

            if(other.CompareTag(targetTag) == false)
                return;

            if(other.TryGetComponent<IHealth>(out IHealth unitHealth) == false)
                return;

            UnitFSMData unitFSMData = owner.FSMBrain.GetAIData<UnitFSMData>();
            EAttackAttribute attackAttribute = unitFSMData.attackAttribute;
            unitFSMData.attackAttribute = EAttackAttribute.Normal;

            unitHealth.Attack(owner, attackData);
            _ = new PlayHitFeedback(attackData, unitFSMData.attackAttribute, transform.position, Vector3.zero, unitFSMData.forwardDirection);

            unitFSMData.attackAttribute = attackAttribute;

            switch(collisionType)
            {
                case ESimpleProjectileCollisionType.Despawm:
                    StopAllCoroutines();
                    PoolManager.Despawn(this);
                    break;
                case ESimpleProjectileCollisionType.Collision:
                    CollisionFeedback(other);
                    break;
            }
        }

        private void CollisionFeedback(Collider2D other)
        {
            isReleasing = true;
            Vector2 direction = (other.transform.position - transform.position).normalized;
            Vector2 bounceForce = new Vector2(BOUNCE_FORCE.x + Random.Range(-BOUNCE_FORCE_RANDOMNESS.x, BOUNCE_FORCE_RANDOMNESS.x), BOUNCE_FORCE.y + Random.Range(-BOUNCE_FORCE_RANDOMNESS.y, BOUNCE_FORCE_RANDOMNESS.y));
            projectileRigidbody.linearVelocity = new Vector2(-Mathf.Sign(direction.x) * bounceForce.x, bounceForce.y);
            projectileRigidbody.gravityScale = BOUNCE_GRAVITY_SCALE;

            StopAllCoroutines();
            StartCoroutine(DespawnCoroutine(BOUNCE_LIFE_TIME));
        }

        private IEnumerator DespawnCoroutine(float lifeTime)
        {
            yield return new WaitForSeconds(lifeTime);
            PoolManager.Despawn(this);
        }
    }
}