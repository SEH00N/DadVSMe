using UnityEngine;

namespace DadVSMe
{
    public static class BezierUtility
    {
        // 3차(큐빅) 베지어: P0, P1, P2, P3
        public static Vector3 Cubic(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            t = Mathf.Clamp01(t);
            float u = 1f - t;
            return u * u * u * p0
                  + 3f * u * u * t * p1
                  + 3f * u * t * t * p2
                  + t * t * t * p3;
        }

        // 기본 이징(빠르게-느리게)
        public static float EaseInOut(float t) => t * t * (3f - 2f * t);
    }
}
