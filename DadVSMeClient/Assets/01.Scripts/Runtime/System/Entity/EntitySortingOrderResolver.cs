using System.Collections.Generic;
using UnityEngine;

namespace DadVSMe.Entities
{
    public class EntitySortingOrderResolver : MonoBehaviour
    {
        [SerializeField] EntitySortingOrderProvider entitySortingOrderProvider = null;
        private HashSet<EntitySortingOrderProvider> overlappedSortingOrderProviders = null;

        private void Awake()
        {
            overlappedSortingOrderProviders = new HashSet<EntitySortingOrderProvider>();
        }
        
        private void OnTriggerEnter2D(Collider2D collider)
        {
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

            Debug.Log($"[EntitySortingOrderResolver::OnTriggerEnter2D] {otherSortingOrderProvider.name} added. Time : {Time.time}, Name : {gameObject.name}");

            AddProvider(otherSortingOrderProvider);
            ReorderSortingOrder(otherSortingOrderProvider);
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if(collider.CompareTag(GameDefine.EntitySortingOrderProviderTag) == false)
                return;

            if(collider.gameObject.TryGetComponent<EntitySortingOrderProvider>(out EntitySortingOrderProvider otherSortingOrderProvider) == false)
                return;

            if(otherSortingOrderProvider == entitySortingOrderProvider)
            {
                Debug.LogError($"[EntitySortingOrderResolver::OnTriggerExit2D] self-reference detected.");
                return;
            }

            Debug.Log($"[EntitySortingOrderResolver::OnTriggerExit2D] {otherSortingOrderProvider.name} removed. Time : {Time.time}, Name : {gameObject.name}");

            RemoveProvider(otherSortingOrderProvider);
            ReorderSortingOrder(otherSortingOrderProvider);
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

            int targetSortingOrder = minSortingOrder == int.MaxValue ? 0 : minSortingOrder - 1;
            entitySortingOrderProvider.SetSortingOrder(targetSortingOrder);
        }

        private void AddProvider(EntitySortingOrderProvider sortingOrderProvider)
        {
            sortingOrderProvider.OnSortingOrderChanged -= ReorderSortingOrder;
            sortingOrderProvider.OnSortingOrderChanged += ReorderSortingOrder;
            overlappedSortingOrderProviders.Add(sortingOrderProvider);
        }

        private void RemoveProvider(EntitySortingOrderProvider sortingOrderProvider)
        {
            sortingOrderProvider.OnSortingOrderChanged -= ReorderSortingOrder;
            overlappedSortingOrderProviders.Remove(sortingOrderProvider);
        }
    }
}