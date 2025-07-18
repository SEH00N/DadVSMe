using UnityEngine;

namespace DadVSMe.Entities
{
    [ExecuteAlways]
    public class EntityTransform : MonoBehaviour
    {
        // private float lastUpdatedDelta = 0f;
        // private float targetDepth = 0f;

        // private void FixedUpdate()
        // {
        //     if(lastUpdatedDelta == transform.position.y)
        //         return;

        //     lastUpdatedDelta = transform.position.y;
        //     UpdateDepth(lastUpdatedDelta);
        // }

        // #if UNITY_EDITOR
        // private void Update()
        // {
        //     if(Application.isPlaying)
        //         return;

        //     if (lastUpdatedDelta != transform.position.y)
        //     {
        //         lastUpdatedDelta = transform.position.y;
        //         UpdateDepth(lastUpdatedDelta);
        //     }

        //     if(targetDepth != transform.position.z)
        //     {
        //         transform.position = new Vector3(transform.position.x, transform.position.y, targetDepth);
        //     }
        // }
        // #endif

        // private void LateUpdate()
        // {
        //     if(targetDepth == transform.position.z)
        //         return;

        //     transform.position = new Vector3(transform.position.x, transform.position.y, targetDepth);
        // }

        // private void UpdateDepth(float delta)
        // {
        //     targetDepth = delta * 0.01f;
        //     // Vector3 position = transform.position;
        //     // position.z = delta * 0.01f;
        //     // transform.position = position;
        // }
    }
}
