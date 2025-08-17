using UnityEngine;

namespace DadVSMe.Enemies
{
    [CreateAssetMenu(menuName = "DadVSMe/EnemyData/SimpleEnemyData")]
    public class SimpleEnemyData : EnemyDataBase
    {
        public ESimpleEnemyType enemyType;
    }
}
