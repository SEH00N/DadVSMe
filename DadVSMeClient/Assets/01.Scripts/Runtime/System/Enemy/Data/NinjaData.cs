using UnityEngine;

namespace DadVSMe.Enemies
{
    [CreateAssetMenu(menuName = "DadVSMe/EntityData/NinjaData")]
    public class NinjaData : KidEnemyData
    {
        [Header("Ninja")]
        public float jumpAttackCooltime = 3f;
    }
}
