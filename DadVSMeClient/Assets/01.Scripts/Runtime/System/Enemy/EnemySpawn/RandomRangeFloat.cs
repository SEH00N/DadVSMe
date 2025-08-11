using System;
using UnityEngine;

namespace DadVSMe
{
    [Serializable]
    public struct RandomRangeFloat
    {
        public float min;
        public float max;

        public RandomRangeFloat(float min, float max)
        {
            this.min = min;
            this.max = max;
        }

        public void Sort()
        {
            if (max < min) (min, max) = (max, min);
        }

        public float Next()
        {
            float lo = Mathf.Min(min, max);
            float hi = Mathf.Max(min, max);
            return UnityEngine.Random.Range(lo, hi);
        }
    }
}
