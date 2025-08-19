using UnityEngine;

namespace DadVSMe
{
    public class ItemMagnetSkill : AutoActiveSkill
    {
        private float checkRadius;
        private float levelUpIncreaseRate;
        private float magnetSpeedMultiplier;

        public ItemMagnetSkill(float cooltime, float checkRadius, float levelUpIncreaseRate, float magnetSpeedMultiplier) : base(cooltime)
        {
            this.checkRadius = checkRadius;
            this.levelUpIncreaseRate = levelUpIncreaseRate;
            this.magnetSpeedMultiplier = magnetSpeedMultiplier;
        }

        public override void Execute()
        {
            base.Execute();

            Vector2 spawnPoint = ownerComponent.transform.position;
            Collider2D[] cols = Physics2D.OverlapCircleAll(spawnPoint, checkRadius);

            if (cols.Length == 0)
                return;

            foreach (var col in cols)
            {
                if (col.gameObject.TryGetComponent<Item>(out Item item))
                {
                    item.transform.position = Vector3.Lerp(
                        item.transform.position, ownerComponent.transform.position, Time.deltaTime * magnetSpeedMultiplier);
                }
            }
        }

        public override void LevelUp()
        {
            base.LevelUp();

            checkRadius += levelUpIncreaseRate;
        }
    }
}