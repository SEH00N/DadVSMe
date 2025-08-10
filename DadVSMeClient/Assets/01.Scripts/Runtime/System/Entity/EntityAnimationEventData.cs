using UnityEngine;

namespace DadVSMe.Entities
{
    [CreateAssetMenu(menuName = "DadVSMe/Entity/EntityAnimationEventData")]
    public class EntityAnimationEventData : ScriptableObject
    {
        public EEntityAnimationEventType eventType = EEntityAnimationEventType.None;
    }
}