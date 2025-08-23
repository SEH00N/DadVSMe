using System;
using Cysharp.Threading.Tasks;
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
        private PoolReference poolReference;
        private BezierMover bezierMover;

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
            bezierMover = GetComponent<BezierMover>();
            poolReference = GetComponent<PoolReference>();
        }

        void Start()
        {
            bezierMover.onArrivedEvent.AddListener(OnArrived);
        }

        private void OnArrived(Transform target)
        {
            if (target == null)
            {
                DespawnInternal();
                return;
            }

            if (target.TryGetComponent<UnitHealth>(out UnitHealth targetHealth))
            {
                targetHealth.Attack(instigator, attackData);

                Vector3 direction = (target.transform.position - transform.position).normalized;
                _ = new PlayAttackFeedback(attackData, EAttackAttribute.Normal, transform.position, Vector3.zero, (int)Mathf.Sign(direction.x));
            }

            DespawnInternal();
        }

        void Update()
        {
            if (target == null)
            {
                DespawnInternal();
                return;
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
            bezierMover.LaunchAsync(target.transform).Forget();
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