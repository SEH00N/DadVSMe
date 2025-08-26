using System;
using Cysharp.Threading.Tasks;
using DadVSMe.Entities;
using UnityEngine;

namespace DadVSMe
{
    // Dad == Dead
    public class Deadline : MonoBehaviour, IAttacker
    {
        [SerializeField] JuggleAttackData deadlineJuggleAttackData = null;
        [SerializeField] float _moveSpeed;

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

            // Play Dad Springing Up Animation

            // Temp
            await UniTask.Delay(TimeSpan.FromSeconds(1.5f));

            // Player Juggling
            GameInstance.GameCycle.MainPlayer.UnitHealth.Attack(this, deadlineJuggleAttackData);

            // Release Camera
            _ = new ChangeCinemachineCamera(GameInstance.GameCycle.MainCinemachineCamera);

            // Go! Text
        }
    }
}
