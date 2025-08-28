using Cysharp.Threading.Tasks;
using DadVSMe.Entities;
using H00N.Resources.Addressables;
using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe.Items
{
    public abstract class Item : PoolReference
    {
        [SerializeField] AddressableAsset<AudioClip> onCollectedSound = null;

        private BezierMover bezierMover;

        protected override void Awake()
        {
            base.Awake();

            bezierMover = GetComponent<BezierMover>();
            bezierMover.onArrivedEvent.AddListener(HandleArrived);
        }

        public void Initialize()
        {
            onCollectedSound.InitializeAsync().Forget();
        }


        public void Collect(Transform performer)
        {
            if (bezierMover.IsRunning)
                return;
            
            bezierMover.LaunchAsync(performer).Forget();
        }

        private void HandleArrived(Transform target)
        {
            if (target.TryGetComponent<Entity>(out Entity entity) == false)
                return;

            OnCollected(entity);
            PoolManager.Despawn(this);
        }

        protected virtual void OnCollectedInternal(Entity interactor) { }
        private void OnCollected(Entity interactor)
        {
            _ = new PlaySound(onCollectedSound);
            OnCollectedInternal(interactor);
        }
    }
}
