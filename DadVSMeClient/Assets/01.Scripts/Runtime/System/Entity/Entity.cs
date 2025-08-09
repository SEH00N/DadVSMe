using UnityEngine;

namespace DadVSMe.Entities
{
    public class Entity : MonoBehaviour
    {
        [SerializeField] protected EntityAnimator entityAnimator = null;

        public virtual void Initialize()
        {
            entityAnimator.Initialize();
        }
    }
}
