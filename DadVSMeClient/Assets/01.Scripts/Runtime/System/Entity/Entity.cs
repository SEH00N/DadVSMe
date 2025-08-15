using UnityEngine;

namespace DadVSMe.Entities
{
    public class Entity : MonoBehaviour
    {
        private const float Z_ORDER_OFFSET = 1f;

        [Header("Entity")]
        [SerializeField] protected bool staticEntity = false;
        [SerializeField] protected EntityAnimator entityAnimator = null;
        [SerializeField] protected EntitySortingOrderResolver sortingOrderResolver = null;

        private float currentDepth = 0f;

        private IEntityData dataInfo;
        public IEntityData DataInfo => dataInfo;

        public virtual void Initialize(IEntityData data)
        {
            if(staticEntity)
                return;

            dataInfo = data;
            entityAnimator.Initialize();
        }

        protected virtual void LateUpdate()
        {
            if(staticEntity)
                return;

            float targetDepth = transform.position.y * Z_ORDER_OFFSET;
            if(currentDepth == targetDepth)
                return;

            currentDepth = targetDepth;
            transform.position = new Vector3(transform.position.x, transform.position.y, currentDepth);
        }

        public void AddChildSortingOrderResolver(EntitySortingOrderResolver child)
        {
            sortingOrderResolver.AddChild(child);
        }

        public void RemoveChildSortingOrderResolver(EntitySortingOrderResolver child)
        {
            sortingOrderResolver.RemoveChild(child);
        }
    }
}
