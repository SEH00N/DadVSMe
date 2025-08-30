using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        private IAttackData attackData;
        private IAttackFeedbackDataContainer feedbackDataContainer;

        private PoolReference poolReference;
        private BezierMover bezierMover;

        private Unit instigator;
        private Unit target;

        [SerializeField]
        private float moveSpeed;
        [SerializeField]
        private GameObject staticVisual;
        [SerializeField]
        private List<TrailRenderer> dynamicVisuals;
        [SerializeField] 
        private UnityEvent onDespawnEvent;

        public PoolReference PoolReference => poolReference;

        void Awake()
        {
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
                DespawnInternal();
                Vector3 direction = (target.transform.position - transform.position).normalized;
                
                UnitFSMData unitFSMData = instigator.FSMBrain.GetAIData<UnitFSMData>();
                EAttackAttribute attackAttribute = unitFSMData.attackAttribute;
                unitFSMData.attackAttribute = EAttackAttribute.Crazy;
                
                targetHealth.Attack(instigator, attackData);
                _ = new PlayHitFeedback(feedbackDataContainer, unitFSMData.attackAttribute, transform.position, Vector3.zero, (int)Mathf.Sign(direction.x));

                unitFSMData.attackAttribute = attackAttribute;
            }
        }

        void Update()
        {
            if (target == null)
            {
                DespawnInternal();
                return;
            }

            float angleInRadian = Mathf.Atan2(bezierMover.Forward2D.y, bezierMover.Forward2D.x);
            float degree = angleInRadian * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, degree);
        }

        public void SetInstigator(Unit instigator, IAttackData attackData, IAttackFeedbackDataContainer feedbackDataContainer)
        {
            this.instigator = instigator;
            this.attackData = attackData;
            this.feedbackDataContainer = feedbackDataContainer;
        }

        public void SetTarget(Unit target)
        {
            this.target = target;
        }

        public async Task Launch()
        {
            bezierMover.LaunchAsync(target.transform).Forget();

            staticVisual.SetActive(false);
            foreach (var dynamicVisual in dynamicVisuals)
                dynamicVisual.Clear();

            await UniTask.Delay(System.TimeSpan.FromSeconds(4f));
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