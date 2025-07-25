using UnityEngine;

namespace DadVSMe.Inputs
{
    public class DoublePress
    {
        private readonly float threshold;
        private float lastPressedTime = float.MinValue;

        public DoublePress(float threshold)
        {
            this.threshold = threshold;
        }

        public bool Press()
        {
            bool result = Time.time - lastPressedTime < threshold;
            lastPressedTime = Time.time;
            return result;
        }
    }
}