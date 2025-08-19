using DadVSMe.Entities;
using DadVSMe.Players;
using H00N.Resources.Pools;
using UnityEngine;

namespace DadVSMe
{
    public class Experience : Item
    {
        [SerializeField] private int amount;
        public int Amount => amount;

        private PoolReference poolReference;

        void Awake()
        {
            poolReference = GetComponent<PoolReference>();
        }

        public override void Interact(Entity interactor)
        {
            Player player = interactor as Player;
            if (player != null)
                player.GetExp(amount);
                
            PoolManager.Despawn(poolReference);
        }
    }
}
