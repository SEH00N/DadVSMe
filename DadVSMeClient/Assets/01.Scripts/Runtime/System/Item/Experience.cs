using DadVSMe.Entities;
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

        public override void Use(Unit user)
        {
            user.GetComponent<UnitSkillComponent>().GetExperience(this);
            PoolManager.Despawn(poolReference);
        }
    }
}
