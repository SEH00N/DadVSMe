using Cysharp.Threading.Tasks;
using DadVSMe.Entities;
using DadVSMe.Players;
using H00N.Resources.Addressables;
using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe
{
    public class Experience : Item
    {
        [SerializeField] AddressableAsset<AudioClip> sound;

        [SerializeField] private int amount;
        public int Amount => amount;

        private PoolReference poolReference;
        private BezierMover bezierMover;

        void Awake()
        {
            poolReference = GetComponent<PoolReference>();
            bezierMover = GetComponent<BezierMover>();
            sound.InitializeAsync().Forget();

            bezierMover.onArrivedEvent.AddListener(OnArriaved);
        }

        public override void Interact(Entity interactor)
        {
            Player player = interactor as Player;
            if (player != null)
                player.GetExp(amount);

            _ = new PlaySound(sound);
        }

        public override void MagnetMove(Transform target)
        {
            if(bezierMover.IsRunning == false)
                bezierMover.LaunchAsync(target).Forget();
        }

        private void OnArriaved(Transform target)
        {
            if (target.TryGetComponent<Entity>(out Entity entity))
            {
                Interact(entity);

                PoolManager.Despawn(poolReference);

            }
        }
    }
}
