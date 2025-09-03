using System;
using H00N.Resources.Pools;
using ShibaInspector.Attributes;
using UnityEngine;

namespace DadVSMe.Entities
{
    public class Entity : PoolReference
    {
        private const float Z_ORDER_OFFSET = 1f;

        [Header("Entity")]
        [SerializeField] protected bool staticEntity = false;
        [SerializeField] protected EntityAnimator entityAnimator = null;
        [SerializeField] protected EntitySortingOrderResolver sortingOrderResolver = null;

        public event Action<IEntityData> OnInitializedEvent = null;
        public event Action OnDespawnEvent = null;

        private float currentDepth = 0f;

        private IEntityData dataInfo;
        public IEntityData DataInfo => dataInfo;

        public bool StaticEntity => staticEntity;

        private void Start()
        {
            ReorderDepth();
        }

        public void Initialize(IEntityData data)
        {
            if (staticEntity)
                return;

            InitializeInternal(data);
            OnInitializedEvent?.Invoke(data);
        }

        protected virtual void InitializeInternal(IEntityData data)
        {
            dataInfo = data;
            entityAnimator.Initialize();
        }

        protected virtual void LateUpdate()
        {
            if (staticEntity)
                return;

            ReorderDepth();
        }

        private void ReorderDepth()
        {
            float targetDepth = transform.position.y * Z_ORDER_OFFSET;
            if (currentDepth == targetDepth)
                return;

            currentDepth = targetDepth;
            transform.position = new Vector3(transform.position.x, transform.position.y, currentDepth);
        }

        public void AddChildSortingOrderResolver(Entity child) => AddChildSortingOrderResolver(child.sortingOrderResolver);
        public void AddChildSortingOrderResolver(EntitySortingOrderResolver child)
        {
            sortingOrderResolver.AddChild(child);
        }

        public void RemoveChildSortingOrderResolver(Entity child) => RemoveChildSortingOrderResolver(child.sortingOrderResolver);
        public void RemoveChildSortingOrderResolver(EntitySortingOrderResolver child)
        {
            sortingOrderResolver.RemoveChild(child);
        }

        protected override void DespawnInternal()
        {
            base.DespawnInternal();
            OnDespawnEvent?.Invoke();
        }
    }
}
