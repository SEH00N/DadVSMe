using ShibaInspector.Attributes;
using UnityEngine;

namespace DadVSMe.Entities
{
    public class Entity : MonoBehaviour
    {
        [Header("Entity")]
        [SerializeField] bool staticEntity = false;
        [SerializeField] protected EntityAnimator entityAnimator = null;
        [SerializeField] protected EntitySortingOrderResolver sortingOrderResolver = null;

        public virtual void Initialize()
        {
            if(staticEntity)
                return;

            entityAnimator.Initialize();
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
