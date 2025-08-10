using UnityEngine;

namespace DadVSMe.Entities
{
    [CreateAssetMenu(fileName = "AttackData", menuName = "DadVSMe/Entity/AttackData")]
    public class AttackData : ScriptableObject, IAttackData
    {
        [SerializeField] int damage = 5;
        public int Damage => damage;

        [SerializeField] EAttackFeedback attackFeedback = EAttackFeedback.NormalHit1;
        public EAttackFeedback AttackFeedback => attackFeedback;

        [SerializeField] float attackFeedbackValue = 1f;
        public float AttackFeedbackValue => attackFeedbackValue;
    }
}
