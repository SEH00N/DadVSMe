using UnityEngine;

namespace DadVSMe.Enemies
{
    [CreateAssetMenu(menuName = "DadVSMe/EntityData/ATVEnemyData")]
    public class ATVEnemyData : EnemyDataBase
    {
        [Header("ATV Enemy")]
        public float atvCooltime = 1f;
    }
}