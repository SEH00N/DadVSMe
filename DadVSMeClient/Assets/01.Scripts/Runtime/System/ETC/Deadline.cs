using System;
using Cysharp.Threading.Tasks;
using DadVSMe.Entities;
using UnityEngine;

namespace DadVSMe
{
    // Dad == Dead
    public class Deadline : MonoBehaviour, IAttacker
    {
        private const float BOSS_CLEAR_DIRECTING_CAMERA_RELEASE_BLEND_DURATION = 0.5f;
        private const float BOSS_CLEAR_DIRECTING_CAMERA_RELEASE_DURATION = 1f;

        private const string BOSS_CLEAR_DIRECTING_JUMP_ANIMATION_NAME = "Jump";
        private const float BOSS_CLEAR_DIRECTING_JUMP_TRIGGER_DURATION = 0.1f;
        private const float BOSS_CLEAR_DIRECTING_JUMP_TRIGGER_TIME_FREEZE_DELAY = 0.1f;
        private const float BOSS_CLEAR_DIRECTING_JUMP_TRIGGER_TIME_FREEZE_DURATION = 0.1f;
        private const float BOSS_CLEAR_DIRECTING_JUMP_TRIGGER_TIME_FREEZE_SCALE = 0.35f;

        private const float BOSS_CLEAR_DIRECTING_CAMERA_SHAKE_DURATION = 0.325f;
        private const float BOSS_CLEAR_DIRECTING_CAMERA_SHAKE_AMPLITUDE = 14f;
        private const float BOSS_CLEAR_DIRECTING_CAMERA_SHAKE_FREQUENCY = 8f;

        private const float BOSS_CLEAR_DIRECTING_FINISHING_DURATION = 3f;

        [SerializeField] Animator deadlineAnimator = null;
        [SerializeField] JuggleAttackData deadlineJuggleAttackData = null;
        [SerializeField] Transform deadlineJumpTriggerTargetTransform = null;

        [Space(10f)]
        [SerializeField] float _moveSpeed;

        Transform IAttacker.AttackerTransform => transform;
        EAttackAttribute IAttacker.AttackAttribute => EAttackAttribute.Normal;
        float IAttacker.AttackPower => 1f;

        public void Initialize()
        {
            
        }

        private void FixedUpdate()
        {
            Vector3 movement = transform.right * (_moveSpeed * Time.fixedDeltaTime);
            transform.position += movement;
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public async UniTask PlayBossClearDirecting()
        {
            // Release Camera
            _ = new ChangeCinemachineCamera(GameInstance.GameCycle.MainCinemachineCamera, BOSS_CLEAR_DIRECTING_CAMERA_RELEASE_BLEND_DURATION);

            await UniTask.Delay(TimeSpan.FromSeconds(BOSS_CLEAR_DIRECTING_CAMERA_RELEASE_DURATION), ignoreTimeScale: true);

            // Deadline Positioning
            Vector3 offset = deadlineJumpTriggerTargetTransform.position - GameInstance.GameCycle.MainPlayer.transform.position;
            Vector3 currentPosition = transform.position;
            currentPosition.x = transform.position.x - offset.x;
            transform.position = currentPosition;

            // Deadline Springing Up Animation
            SetActive(true);
            deadlineAnimator.Play(BOSS_CLEAR_DIRECTING_JUMP_ANIMATION_NAME);

            await UniTask.Delay(TimeSpan.FromSeconds(BOSS_CLEAR_DIRECTING_JUMP_TRIGGER_DURATION), ignoreTimeScale: true);

            // Player Positioning
            Vector3 playerPosition = GameInstance.GameCycle.MainPlayer.transform.position;
            playerPosition.y = deadlineJumpTriggerTargetTransform.position.y;
            GameInstance.GameCycle.MainPlayer.transform.position = playerPosition;

            // Player Juggling
            GameInstance.GameCycle.MainPlayer.UnitHealth.Attack(this, deadlineJuggleAttackData);
            _ = new PlayAttackFeedback(deadlineJuggleAttackData, EAttackAttribute.Normal, GameInstance.GameCycle.MainPlayer.transform.position, Vector3.zero, 1);
            _ = new ShakeCamera(GameInstance.GameCycle.MainCinemachineCamera, BOSS_CLEAR_DIRECTING_CAMERA_SHAKE_DURATION, BOSS_CLEAR_DIRECTING_CAMERA_SHAKE_AMPLITUDE, BOSS_CLEAR_DIRECTING_CAMERA_SHAKE_FREQUENCY);
            
            // Attack Feedback Time Freezing
            await UniTask.Delay(TimeSpan.FromSeconds(BOSS_CLEAR_DIRECTING_JUMP_TRIGGER_TIME_FREEZE_DELAY), ignoreTimeScale: true);
            TimeManager.SetTimeScale(BOSS_CLEAR_DIRECTING_JUMP_TRIGGER_TIME_FREEZE_SCALE, true);
            await UniTask.Delay(TimeSpan.FromSeconds(BOSS_CLEAR_DIRECTING_JUMP_TRIGGER_TIME_FREEZE_DURATION), ignoreTimeScale: true);
            TimeManager.SetTimeScale(GameDefine.DEFAULT_TIME_SCALE, true);

            // Go! Text

            await UniTask.Delay(TimeSpan.FromSeconds(BOSS_CLEAR_DIRECTING_FINISHING_DURATION), ignoreTimeScale: true);
        }
    }
}
