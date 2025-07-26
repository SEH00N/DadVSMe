using System;
using System.Collections.Generic;
using UnityEngine;

namespace DadVSMe.Players.Animations
{
    public class PlayerAnimationEventListener : MonoBehaviour
    {
        private Dictionary<EPlayerAnimationEventType, Action<PlayerAnimationEventData>> eventListeners = null;

        private void Awake()
        {
            eventListeners = new Dictionary<EPlayerAnimationEventType, Action<PlayerAnimationEventData>>();
        }

        public void OnAnimationEvent(PlayerAnimationEventData eventData)
        {
            if(eventData == null)
                return;

            if(eventData.eventType == EPlayerAnimationEventType.None)
                return;

            if(eventListeners == null)
                return;

            if(eventListeners.TryGetValue(eventData.eventType, out Action<PlayerAnimationEventData> action) == false)
                return;

            action?.Invoke(eventData);
        }

        public void AddEventListener(EPlayerAnimationEventType eventType, Action<PlayerAnimationEventData> action)
        {
            if(eventListeners == null)
                return;

            if(eventListeners.TryGetValue(eventType, out Action<PlayerAnimationEventData> listener) == false || listener == null)
                eventListeners[eventType] = action;
            else
                eventListeners[eventType] += action;
        }

        public void RemoveEventListener(EPlayerAnimationEventType eventType, Action<PlayerAnimationEventData> action)
        {
            if(eventListeners == null)
                return;

            if(eventListeners.TryGetValue(eventType, out Action<PlayerAnimationEventData> listener) == false || listener == null)
                return;

            eventListeners[eventType] -= action;
        }

        public void Clear()
        {
            if(eventListeners == null)
                return;

            eventListeners.Clear();
        }
    }
}