using System;
using Cysharp.Threading.Tasks;
using DadVSMe.Entities;
using UnityEngine;

namespace DadVSMe
{
    // Dad == Dead
    public class Deadline : MonoBehaviour, IAttacker
    {
        [SerializeField] Animator deadlineAnimator = null;
        [SerializeField] JuggleAttackData deadlineJuggleAttackData = null;
        [SerializeField] float _moveSpeed;
        [SerializeField] float test = 13f;

        public Transform AttackerTransform => transform;
        public EAttackAttribute AttackAttribute => EAttackAttribute.Normal;
        public float AttackPower => 1f;

        public void Initialize()
        {
            
        }

        private void Update()
        {
            var moveValue = transform.right * _moveSpeed * Time.deltaTime;

            transform.position += moveValue;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("Player"))
            {
                Debug.Log("GameEnd");
            }
        }

        public async UniTask PlayBossClearDirecting()
        {
            // Camera Shaking


            // Release Camera
            _ = new ChangeCinemachineCamera(GameInstance.GameCycle.MainCinemachineCamera, 0.5f);

            await UniTask.Delay(TimeSpan.FromSeconds(1f), ignoreTimeScale: true);

            Vector3 currentPosition = transform.position;
            currentPosition.x = GameInstance.GameCycle.MainPlayer.transform.position.x - test;
            transform.position = currentPosition;

            // Play Dad Springing Up Animation
            deadlineAnimator.Play("Jump");

            await UniTask.Delay(TimeSpan.FromSeconds(0.1f), ignoreTimeScale: true);

            // Player Juggling
            GameInstance.GameCycle.MainPlayer.UnitHealth.Attack(this, deadlineJuggleAttackData);
            _ = new PlayAttackFeedback(deadlineJuggleAttackData, EAttackAttribute.Normal, GameInstance.GameCycle.MainPlayer.transform.position, Vector3.zero, 1);
            
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f), ignoreTimeScale: true);
            TimeManager.SetTimeScale(0.3f, true);
            await UniTask.Delay(TimeSpan.FromSeconds(0.2f), ignoreTimeScale: true);
            TimeManager.SetTimeScale(GameDefine.DEFAULT_TIME_SCALE, true);

            // Go! Text

            await UniTask.Delay(TimeSpan.FromSeconds(2f), ignoreTimeScale: true);
        }
    }
}
