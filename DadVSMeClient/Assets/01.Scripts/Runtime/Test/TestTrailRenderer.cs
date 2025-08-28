using UnityEngine;

namespace DadVSMe.Tests
{
    public class TestTrailRenderer : MonoBehaviour
    {
        [SerializeField] TrailRenderer trailRenderer;
        [SerializeField] float testValue = 1f;

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                trailRenderer.AddPosition(transform.position + (transform.up * testValue));
            }
        }
    }
}