using DadVSMe.Entities;
using H00N.Resources.Pools;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DadVSMe
{
    public class GuidedOrb : Entity
    {
        [SerializeField]
        private AttackDataBase attackData;

        private UnitMovement movement;
        private PoolReference poolReference;

        private Unit instigator;
        private Unit target;

        [SerializeField]
        private float moveSpeed;

        void Awake()
        {
            Initialize(null);
        }

        void Start()
        {
            entityAnimator.AddAnimationEventListener(EEntityAnimationEventType.Trigger, Despawn);
        }

        public override void Initialize(IEntityData data)
        {
            base.Initialize(data);

            movement = GetComponent<UnitMovement>();
            poolReference = GetComponent<PoolReference>();
        }

        void Update()
        {
            if (target)
            {
                Vector2 moveDirection = (target.transform.position - transform.position).normalized;
                movement.SetMovementVelocity(moveDirection * moveSpeed);
            }
        }

        public void SetInstigator(Unit instigator)
        {
            this.instigator = instigator;
        }

        public void SetTarget(Unit target)
        {
            this.target = target;
        }

        public void Launch()
        {
            movement.SetActive(true);
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject != target.gameObject)
                return;

            if (collision.gameObject.TryGetComponent<UnitHealth>(out UnitHealth targetHealth))
            {
                targetHealth.Attack(instigator, attackData);
            }

            movement.SetActive(false);
            entityAnimator.PlayAnimation("GuidedOrbHit");
        }

        public void Despawn(EntityAnimationEventData animData)
        {
            PoolManager.Despawn(poolReference);
        }
    }
}