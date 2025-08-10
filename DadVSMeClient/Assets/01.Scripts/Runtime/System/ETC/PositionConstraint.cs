using ShibaInspector.Attributes;
using UnityEngine;

namespace DadVSMe
{
    public class PositionConstraint : MonoBehaviour
    {
        [SerializeField] Transform target = null;
        [SerializeField] bool zMinimum = false;
        [SerializeField] float minZ = 0f;

        [SerializeField] bool zMaximum = false;
        [SerializeField] float maxZ = 0f;

        private void LateUpdate()
        {
            float targetZ = target.position.z;
            if(zMinimum)
                targetZ = Mathf.Max(targetZ, minZ);
            if(zMaximum)
                targetZ = Mathf.Min(targetZ, maxZ);

            transform.position = new Vector3(target.position.x, target.position.y, targetZ);
        }
    }
}
