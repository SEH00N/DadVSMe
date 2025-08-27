using System;
using Cysharp.Threading.Tasks;
using DadVSMe.Entities;
using UnityEngine;

namespace DadVSMe
{
    public class DeadlineCollision : MonoBehaviour, IAttacker
    {
        private const float CAMERA_SHAKE_DURATION = 0.325f;
        private const float CAMERA_SHAKE_AMPLITUDE = 14f;
        private const float CAMERA_SHAKE_FREQUENCY = 8f;

        [SerializeField] JuggleAttackData deadlineCollisionAttackData = null;

        Transform IAttacker.AttackerTransform => transform;
        EAttackAttribute IAttacker.AttackAttribute => EAttackAttribute.Normal;
        float IAttacker.AttackPower => 1f;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.CompareTag(GameDefine.PlayerTag) == false)
                return;

            if(other.TryGetComponent<UnitHealth>(out UnitHealth unitHealth) == false)
                return;

            unitHealth.Attack(this, deadlineCollisionAttackData);
            _ = new PlayHitFeedback(deadlineCollisionAttackData, EAttackAttribute.Normal, GameInstance.GameCycle.MainPlayer.transform.position, Vector3.zero, 1);
            _ = new ShakeCamera(GameInstance.GameCycle.MainCinemachineCamera, CAMERA_SHAKE_DURATION, CAMERA_SHAKE_AMPLITUDE, CAMERA_SHAKE_FREQUENCY);
        }
    }
}
