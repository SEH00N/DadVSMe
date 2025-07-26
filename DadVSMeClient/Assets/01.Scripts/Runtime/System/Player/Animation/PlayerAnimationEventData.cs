using UnityEngine;

namespace DadVSMe.Players.Animations
{
    [CreateAssetMenu(menuName = "CreateSO/Player/PlayerAnimationEventData")]
    public class PlayerAnimationEventData : ScriptableObject
    {
        public EPlayerAnimationEventType eventType = EPlayerAnimationEventType.None;
    }
}