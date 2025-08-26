using UnityEngine;
using UnityEngine.Events;

namespace DadVSMe.UI
{
    public class AnimationUI : MonoBehaviour
    {
        [SerializeField] UnityEvent onAnimationTriggerEvent = null;

        public void OnAnimationTrigger()
        {
            onAnimationTriggerEvent?.Invoke();
        }
    }
}
