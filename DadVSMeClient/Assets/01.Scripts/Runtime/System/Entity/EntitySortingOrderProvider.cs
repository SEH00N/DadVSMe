using System;
using UnityEngine;

namespace DadVSMe.Entities
{
    public class EntitySortingOrderProvider : MonoBehaviour
    {
        [SerializeField] SpriteRenderer entitySpriteRenderer = null;

        [SerializeField] EntitySortingOrderResolver entitySortingOrderResolver = null;
        public EntitySortingOrderResolver EntitySortingOrderResolver => entitySortingOrderResolver;

        public event Action<EntitySortingOrderProvider> OnSortingOrderChanged;

        public int GetSortingOrder()
        {
            return entitySpriteRenderer.sortingOrder;
        }

        public void SetSortingOrder(int sortingOrder)
        {
            entitySpriteRenderer.sortingOrder = sortingOrder;
            OnSortingOrderChanged?.Invoke(this);
        }
    }
}
