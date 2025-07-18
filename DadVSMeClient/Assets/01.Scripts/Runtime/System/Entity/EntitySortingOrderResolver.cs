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

            if(overlappedSortingOrderProviders.Contains(otherSortingOrderProvider) == true)
                return;

            overlappedSortingOrderProviders.Add(otherSortingOrderProvider);
            otherSortingOrderProvider.OnSortingOrderChanged += ReorderSortingOrder;
            ReorderSortingOrder(otherSortingOrderProvider);
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if(collider.CompareTag(GameDefine.EntitySortingOrderProviderTag) == false)
                return;

            if(collider.gameObject.TryGetComponent<EntitySortingOrderProvider>(out EntitySortingOrderProvider otherSortingOrderProvider) == false)
                return;

            overlappedSortingOrderProviders.Remove(otherSortingOrderProvider);
            otherSortingOrderProvider.OnSortingOrderChanged -= ReorderSortingOrder;
            ReorderSortingOrder(otherSortingOrderProvider);
        }

        private void ReorderSortingOrder(EntitySortingOrderProvider performer)
        {
            // if performer is cross-referenced, do not reorder sorting order for prevent infinite loop.
            if(performer.IsCrossReferenced(entitySortingOrderProvider) == true)
                return;

            int minSortingOrder = int.MaxValue; // default sorting should be zero
            foreach(var otherSortingOrderProvider in overlappedSortingOrderProviders)
                minSortingOrder = Mathf.Min(minSortingOrder, otherSortingOrderProvider.GetSortingOrder());

            int targetSortingOrder = minSortingOrder == int.MaxValue ? 0 : minSortingOrder - 1;
            entitySortingOrderProvider.SetSortingOrder(targetSortingOrder);
        }

        public bool Contains(EntitySortingOrderProvider sortingOrderProvider)
        {
            return overlappedSortingOrderProviders.Contains(sortingOrderProvider);
        }
    }
}