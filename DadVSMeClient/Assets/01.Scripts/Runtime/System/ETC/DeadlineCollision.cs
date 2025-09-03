using DadVSMe.Entities;
using UnityEngine;

namespace DadVSMe
{
    public class DeadlineCollision : MonoBehaviour, IAttacker
    {
        private const float OFFSET = 0.75f;
        private const float CAMERA_SHAKE_DURATION = 0.325f;
        private const float CAMERA_SHAKE_AMPLITUDE = 14f;
        private const float CAMERA_SHAKE_FREQUENCY = 8f;

        [SerializeField] JuggleAttackData deadlineCollisionPlayerAttackData = null;
        [SerializeField] JuggleAttackData deadlineCollisionEnemyAttackData = null;

        Transform IAttacker.AttackerTransform => transform;
        EAttackAttribute IAttacker.AttackAttribute => EAttackAttribute.Crazy;
        float IAttacker.AttackPower => 1f;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.CompareTag(GameDefine.PlayerTag))
                AttackToTarget(other, deadlineCollisionPlayerAttackData, true);

            if(other.CompareTag(GameDefine.EnemyTag))
                AttackToTarget(other, deadlineCollisionEnemyAttackData, false);
        }

        private void AttackToTarget(Collider2D other, JuggleAttackData attackData, bool shakeCamera)
        {
            if(other.TryGetComponent<IHealth>(out IHealth unitHealth) == false)
                return;

            unitHealth.Attack(this, attackData);
            _ = new PlayHitFeedback(attackData, EAttackAttribute.Crazy, other.transform.position + Vector3.up * OFFSET, Vector3.zero, 1);

            if(shakeCamera)
                _ = new ShakeCamera(GameInstance.GameCycle.MainCinemachineCamera, CAMERA_SHAKE_DURATION, CAMERA_SHAKE_AMPLITUDE, CAMERA_SHAKE_FREQUENCY);
        }
    }
}
