using UnityEngine;

namespace DadVSMe.Entities
{
    [CreateAssetMenu(menuName = "DadVSMe/AttackData/ShootAttackData")]
    public class ShootAttackData : AttackDataBase
    {
        public override EAttackFeedback AttackFeedback => EAttackFeedback.None;
    }
}
