using UnityEngine;

namespace DadVSMe
{
    // Dad == Dead
    public class Deadline : MonoBehaviour
    {
        [SerializeField] float _moveSpeed;
        [SerializeField] Transform _playerBoundary;

        private void Update()
        {
            var moveValue = transform.right * _moveSpeed * Time.deltaTime;

            transform.position += moveValue;
            _playerBoundary.position += moveValue;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("Player"))
            {
                Debug.Log("GameEnd");
            }
        }
    }
}
