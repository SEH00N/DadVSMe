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

        void Awake()
        {
            poolReference = GetComponent<PoolReference>();
            sound.InitializeAsync().Forget();
        }

        public override void Interact(Entity interactor)
        {
            Player player = interactor as Player;
            if (player != null)
                player.GetExp(amount);

            _ = new PlaySound(sound);
            PoolManager.Despawn(poolReference);
        }
    }
}
