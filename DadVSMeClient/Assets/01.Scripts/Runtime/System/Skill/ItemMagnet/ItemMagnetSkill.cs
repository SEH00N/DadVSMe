using DadVSMe.Players;
using UnityEngine;

namespace DadVSMe
{
    public class ItemMagnetSkill : StatUpSkill
    {
        private float levelUpIncreaseRate;
        private Player player;

        public ItemMagnetSkill(float cooltime, float checkRadius, float levelUpIncreaseRate, float magnetSpeedMultiplier) : base()
        {
            this.levelUpIncreaseRate = levelUpIncreaseRate;
        }

        public override void OnRegist(UnitSkillComponent ownerComponent)
        {
            base.OnRegist(ownerComponent);

            player = ownerComponent.GetComponent<Player>();
        }

        public override void Execute()
        {
            base.Execute();

            player.itemFidnRadius += levelUpIncreaseRate;
        }   
    }
}