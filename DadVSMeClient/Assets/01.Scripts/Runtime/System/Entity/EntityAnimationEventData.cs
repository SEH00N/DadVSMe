using UnityEngine;

namespace DadVSMe.Entities
{
    [CreateAssetMenu(menuName = "CreateSO/Entity/EntityAnimationEventData")]
    public class EntityAnimationEventData : ScriptableObject
    {
        public EEntityAnimationEventType eventType = EEntityAnimationEventType.None;
    }
}