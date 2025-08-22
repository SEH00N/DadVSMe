using System;
using UnityEngine;

namespace DadVSMe.UI
{
    public class MonoBehaviourUI : MonoBehaviourUI<IUICallback> { }
    public class MonoBehaviourUI<TCallback> : MonoBehaviour where TCallback : class, IUICallback
    {
        private RectTransform rectTransform = null;
        public RectTransform RectTransform {
            get {
                if(rectTransform == null)
                    rectTransform = transform as RectTransform;
                return rectTransform;
            }
        }

        public event Action OnInitializeEvent = null;
        public event Action OnReleaseEvent = null;

        protected TCallback callback = null;

        protected virtual void Awake() { }

        protected virtual void Initialize(IUICallback callback = null)
        {
            this.callback = callback as TCallback;
            OnInitializeEvent?.Invoke();
        }

        protected virtual void Release() 
        { 
            OnReleaseEvent?.Invoke(); 
        }

        public void StretchRect()
        {
            InitializeTransform();
            RectTransform.anchorMin = Vector2.zero;
            RectTransform.anchorMax = Vector2.one;
            RectTransform.offsetMin = Vector2.zero;
            RectTransform.offsetMax = Vector2.zero;
            RectTransform.sizeDelta = Vector2.zero;
        }

        public void InitializeTransform()
        {
            RectTransform.localPosition = Vector3.zero;
            RectTransform.localScale = Vector3.one;
        }
    }
}
