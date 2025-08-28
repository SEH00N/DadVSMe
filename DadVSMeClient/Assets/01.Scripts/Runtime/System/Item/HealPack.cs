using DadVSMe.Entities;
using UnityEngine;

namespace DadVSMe.Items
{
    public class HealPack : Item
    {
        [SerializeField] int amount;

        protected override void OnCollectedInternal(Entity performer)
        {
            if(performer is Unit unit == false)
                return;

            unit.UnitHealth.Heal(amount);
        }
    }
}
