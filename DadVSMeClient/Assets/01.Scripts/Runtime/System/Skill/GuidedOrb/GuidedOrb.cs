using DadVSMe.Entities;
using H00N.Resources.Pools;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace DadVSMe
{
    public class GuidedOrb : MonoBehaviour, IPoolableBehaviour
    {
        [SerializeField]
        private AttackDataBase attackData;

        private EntityAnimator entityAnimator;
        private UnitMovement movement;
        private PoolReference poolReference;

        private Unit instigator;
        private Unit target;

        [SerializeField]
        private float moveSpeed;
        [SerializeField] 
        private UnityEvent onDespawnEvent;

        public PoolReference PoolReference => poolReference;

        void Awake()
        {
            entityAnimator = GetComponent<EntityAnimator>();
            Initialize();
        }

        void Start()
        {
            entityAnimator.AddAnimationEventListener(EEntityAnimationEventType.Trigger, Despawn);
        }

        public void Initialize()
        {
            movement = GetComponent<UnitMovement>();
            poolReference = GetComponent<PoolReference>();
        }

        void Update()
        {
            if (target == null)
            {
                DespawnInternal();
                return;
            }

            Vector2 moveDirection = (target.transform.position - transform.position).normalized;
            movement.SetMovementVelocity(moveDirection * moveSpeed);
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
            if(target == null)
            {
                DespawnInternal();
                return;
            }

            if (collision.gameObject != target.gameObject)
                return;

            if (collision.gameObject.TryGetComponent<UnitHealth>(out UnitHealth targetHealth))
            {
                targetHealth.Attack(instigator, attackData);

                Vector3 direction = (target.transform.position - transform.position).normalized;
                _ = new PlayAttackFeedback(attackData, EAttackAttribute.Normal, transform.position, Vector3.zero, (int)Mathf.Sign(direction.x));
            }

            movement.SetActive(false);
            entityAnimator.PlayAnimation("GuidedOrbHit");
        }

        public void Despawn(EntityAnimationEventData animData)
        {
            DespawnInternal();
        }

        private void DespawnInternal()
        {
            onDespawnEvent?.Invoke();
            PoolManager.Despawn(poolReference);

        }

        public void OnSpawned()
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        public void OnDespawn() { }
    }
}