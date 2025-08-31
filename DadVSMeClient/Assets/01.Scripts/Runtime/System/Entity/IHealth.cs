using UnityEngine;

namespace DadVSMe.Entities
{
    public interface IHealth
    {
        public Vector3 Position { get; }
        public int CurrentHP { get; }

        public void Attack(IAttacker attacker, IAttackData attackData);
    }
}