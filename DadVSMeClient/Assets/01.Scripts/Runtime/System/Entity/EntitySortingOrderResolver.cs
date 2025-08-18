using System.Collections.Generic;
using UnityEngine;

namespace DadVSMe.Entities
{
    public class EntitySortingOrderResolver : MonoBehaviour
    {
        private const int SORTING_ORDER_OFFSET = 1000;

        [SerializeField] EntitySortingOrderProvider entitySortingOrderProvider = null;
        [SerializeField] List<Collider2D> resolverColliders = null;
        private HashSet<EntitySortingOrderProvider> overlappedSortingOrderProviders = null;
        private HashSet<EntitySortingOrderProvider> childSortingOrderProviders = null;
        
        private EntitySortingOrderResolver parent = null;

        private void Awake()
        {
            overlappedSortingOrderProviders = new HashSet<EntitySortingOrderProvider>();
            childSortingOrderProviders = new HashSet<EntitySortingOrderProvider>();
            parent = null;
        }
        
        private void OnTriggerEnter2D(Collider2D collider)
        {
            if(parent != null)
                return;

            if(collider.CompareTag(GameDefine.EntitySortingOrderProviderTag) == false)
                return;

            if(collider.gameObject.TryGetComponent<EntitySortingOrderProvider>(out EntitySortingOrderProvider otherSortingOrderProvider) == false)
                return;

            if(otherSortingOrderProvider == entitySortingOrderProvider)
            {
                Debug.LogError($"[EntitySortingOrderResolver::OnTriggerEnter2D] self-reference detected.");
                return;
            }

            if(overlappedSortingOrderProviders.Contains(otherSortingOrderProvider) == true)
                return;

            if(childSortingOrderProviders.Contains(otherSortingOrderProvider) == true)
                return;

            AddProvider(otherSortingOrderProvider);
            ReorderSortingOrder(otherSortingOrderProvider);
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if(parent != null)
                return;

            if(collider.CompareTag(GameDefine.EntitySortingOrderProviderTag) == false)
                return;

            if(collider.gameObject.TryGetComponent<EntitySortingOrderProvider>(out EntitySortingOrderProvider otherSortingOrderProvider) == false)
                return;

            if(otherSortingOrderProvider == entitySortingOrderProvider)
            {
                Debug.LogError($"[EntitySortingOrderResolver::OnTriggerExit2D] self-reference detected.");
                return;
            }

            if(childSortingOrderProviders.Contains(otherSortingOrderProvider) == true)
                return;

            RemoveProvider(otherSortingOrderProvider);
            ReorderSortingOrder(otherSortingOrderProvider);
        }

        public void AddChild(EntitySortingOrderResolver child)
        {
            child.resolverColliders.ForEach(collider => collider.enabled = false);
            child.parent = this;
            child.overlappedSortingOrderProviders.Clear();
            if(overlappedSortingOrderProviders.Contains(child.entitySortingOrderProvider))
                RemoveProvider(child.entitySortingOrderProvider);

            childSortingOrderProviders.Add(child.entitySortingOrderProvider);

            entitySortingOrderProvider.OnSortingOrderChangedEvent -= child.entitySortingOrderProvider.SetSortingOrder;
            entitySortingOrderProvider.OnSortingOrderChangedEvent += child.entitySortingOrderProvider.SetSortingOrder;

            ReorderSortingOrder(child.entitySortingOrderProvider);
        }

        public void RemoveChild(EntitySortingOrderResolver child)
        {
            childSortingOrderProviders.Remove(child.entitySortingOrderProvider);

            entitySortingOrderProvider.OnSortingOrderChangedEvent -= child.entitySortingOrderProvider.SetSortingOrder;
            child.entitySortingOrderProvider.SetSortingOrder(0);
            child.parent = null;
            child.resolverColliders.ForEach(collider => collider.enabled = true);
        }

        public EntitySortingOrderResolver GetCurrentSortingOrderResolver()
        {
            if(parent != null)
                return parent.GetCurrentSortingOrderResolver();

            return this;
        }

        private void ReorderSortingOrder(EntitySortingOrderProvider performer)
        {
            // if two colliders overlap, cross-references may occur due to Trigger Event call order issues.
            // to prevent infinite loops due to cross-references, reordering sorting order after resolving the cross-reference. 
            EntitySortingOrderResolver otherSortingOrderResolver = performer.EntitySortingOrderResolver;
            if(otherSortingOrderResolver.overlappedSortingOrderProviders.Contains(entitySortingOrderProvider) == true)
            {
                Debug.LogWarning($"[EntitySortingOrderResolver::ReorderSortingOrder] {entitySortingOrderProvider.name} is cross-referenced.");
                otherSortingOrderResolver.RemoveProvider(entitySortingOrderProvider);
            }

            int minSortingOrder = int.MaxValue; // default sorting should be zero.
            foreach(var otherSortingOrderProvider in overlappedSortingOrderProviders)
                minSortingOrder = Mathf.Min(minSortingOrder, otherSortingOrderProvider.GetSortingOrder());

            int targetSortingOrder = minSortingOrder == int.MaxValue ? 0 : minSortingOrder - SORTING_ORDER_OFFSET;
            entitySortingOrderProvider.SetSortingOrder(targetSortingOrder);
        }

        private void AddProvider(EntitySortingOrderProvider sortingOrderProvider)
        {
            sortingOrderProvider.OnSortingOrderChangedEvent -= ReorderSortingOrder;
            sortingOrderProvider.OnSortingOrderChangedEvent += ReorderSortingOrder;
            overlappedSortingOrderProviders.Add(sortingOrderProvider);
        }

        private void RemoveProvider(EntitySortingOrderProvider sortingOrderProvider)
        {
            sortingOrderProvider.OnSortingOrderChangedEvent -= ReorderSortingOrder;
            overlappedSortingOrderProviders.Remove(sortingOrderProvider);
        }
    }
}