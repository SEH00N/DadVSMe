using UnityEngine;

namespace DadVSMe.Entities
{
    public abstract class AttackDataBase : ScriptableObject, IAttackData
    {
        [SerializeField] int damage = 0;
        public int Damage => damage;

        [Space(10f)]
        [SerializeField] bool isRangeAttack = false;
        public bool IsRangeAttack => isRangeAttack;

        public abstract EAttackFeedback AttackFeedback { get; }
    }
}
