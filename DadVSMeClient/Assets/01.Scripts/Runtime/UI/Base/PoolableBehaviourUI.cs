using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe.UI
{
    public class PoolableBehaviourUI : PoolableBehaviourUI<IUICallback> { }

    [RequireComponent(typeof(PoolReference))]
    public class PoolableBehaviourUI<TCallback> : MonoBehaviourUI<TCallback>, IPoolableBehaviour where TCallback : class, IUICallback
    {
        private PoolReference poolReference = null;
        public PoolReference PoolReference => poolReference;

        protected override void Awake()
        {
            base.Awake();
            poolReference = GetComponent<PoolReference>();
        }

        public virtual void OnSpawned()
        {
        }

        public virtual void OnDespawn()
        {
        }
    }
}
