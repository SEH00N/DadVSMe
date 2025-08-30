using UnityEngine;

namespace DadVSMe.Entities
{
    public interface IJuggleAttackData
    {
        public float JuggleForce { get; }
        public Vector2 JuggleDirection { get; }
    }
}