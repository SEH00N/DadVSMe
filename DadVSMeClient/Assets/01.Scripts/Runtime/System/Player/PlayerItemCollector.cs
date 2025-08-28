using DadVSMe.Entities;
using DadVSMe.Items;
using UnityEngine;

namespace DadVSMe.Players
{
    public class PlayerItemCollector : MonoBehaviour
    {
        private const float FIND_INTERVAL = 0.25f;
        private float timer = 0f;

        private UnitStatData statData;

        public void Initialize(Player player)
        {
            statData = player.FSMBrain.GetAIData<UnitStatData>();
        }

        private void Update()
        {
            timer += Time.deltaTime;
            if (timer < FIND_INTERVAL)
                return;

            timer = 0f;
            FindItem();
        }

        private void FindItem()
        {
            Vector2 spawnPoint = transform.position;
            Collider2D[] cols = Physics2D.OverlapCircleAll(spawnPoint, statData[EUnitStat.ItemMagnetRadius].FinalValue, GameDefine.ITEM_LAYER_MASK);
            
            if (cols.Length == 0)
                return;
            
            foreach (var col in cols)
            {
                if (col.gameObject.TryGetComponent<Item>(out Item item) == false)
                    return;

                item.Collect(transform);
            }
        }
    }
}