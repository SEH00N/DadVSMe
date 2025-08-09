using System;
using System.Collections.Generic;
using UnityEngine;

namespace DadVSMe.Entities
{
    public class EntitySortingOrderProvider : MonoBehaviour
    {
        [SerializeField] List<SpriteRenderer> entitySpriteRenderers = null;

        [SerializeField] EntitySortingOrderResolver entitySortingOrderResolver = null;
        public EntitySortingOrderResolver EntitySortingOrderResolver => entitySortingOrderResolver;

        public event Action<EntitySortingOrderProvider> OnSortingOrderChanged;
        
        private int sortingOrder = 0;

        public int GetSortingOrder()
        {
            return sortingOrder;
        }

        public void SetSortingOrder(int sortingOrder)
        {
            this.sortingOrder = sortingOrder;
            entitySpriteRenderers.ForEach(i => i.sortingOrder = sortingOrder);
            OnSortingOrderChanged?.Invoke(this);
        }
    }
}
