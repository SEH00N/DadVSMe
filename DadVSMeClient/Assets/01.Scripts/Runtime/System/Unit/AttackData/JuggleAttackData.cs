using UnityEngine;

namespace DadVSMe.Entities
{
    [CreateAssetMenu(menuName = "DadVSMe/AttackData/JuggleAttackData")]
    public class JuggleAttackData : AttackDataBase, IJuggleAttackData
    {
        [SerializeField] float juggleForce = 0f;
        public float JuggleForce => juggleForce;

        [SerializeField] Vector2 juggleDirection = Vector2.zero;
        public Vector2 JuggleDirection => juggleDirection;

        public override EAttackFeedback AttackFeedback => EAttackFeedback.Juggle;
    }
}
