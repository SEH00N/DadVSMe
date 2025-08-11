using UnityEngine;

namespace DadVSMe.Entities
{
    [CreateAssetMenu(menuName = "DadVSMe/AttackData/SimpleAttackData")]
    public class SimpleAttackData : AttackDataBase
    {
        [SerializeField] EAttackFeedback attackFeedback = EAttackFeedback.None;
        public override EAttackFeedback AttackFeedback => attackFeedback;
    }
}
