using UnityEngine;

namespace DadVSMe.Enemies
{
    [CreateAssetMenu(menuName = "DadVSMe/EntityData/ButtEnemyData")]
    public class ButtEnemyData : KidEnemyData
    {
        [Header("Butt Enemy")]
        public float buttCooltime = 1f;
    }
}
