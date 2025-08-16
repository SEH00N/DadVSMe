using System;
using System.Collections.Generic;
using UnityEngine;

namespace DadVSMe.Entities
{
    public class EntityAnimationEventListener : MonoBehaviour
    {
        private Dictionary<EEntityAnimationEventType, Action<EntityAnimationEventData>> eventListeners = null;

        private void Awake()
        {
            eventListeners = new Dictionary<EEntityAnimationEventType, Action<EntityAnimationEventData>>();
        }

        public void OnAnimationEvent(EntityAnimationEventData eventData)
        {
            
            if (eventData == null)
                return;
            
            if(eventData.eventType == EEntityAnimationEventType.None)
                return;
            
            if(eventListeners == null)
                return;
            
            if(eventListeners.TryGetValue(eventData.eventType, out Action<EntityAnimationEventData> action) == false)
                return;
            
            action?.Invoke(eventData);
        }

        public void AddEventListener(EEntityAnimationEventType eventType, Action<EntityAnimationEventData> action)
        {
            if(eventListeners == null)
                return;

            if(eventListeners.TryGetValue(eventType, out Action<EntityAnimationEventData> listener) == false || listener == null)
                eventListeners[eventType] = action;
            else
                eventListeners[eventType] += action;
        }

        public void RemoveEventListener(EEntityAnimationEventType eventType, Action<EntityAnimationEventData> action)
        {
            if(eventListeners == null)
                return;

            if(eventListeners.TryGetValue(eventType, out Action<EntityAnimationEventData> listener) == false || listener == null)
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