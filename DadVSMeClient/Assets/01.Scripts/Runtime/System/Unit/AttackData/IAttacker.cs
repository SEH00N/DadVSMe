using UnityEngine;

namespace DadVSMe.Entities
{
    public interface IAttacker
    {
        public Transform AttackerTransform { get; }
        public EAttackAttribute AttackAttribute { get; }
        public float AttackPower { get; }
    }
}