using Cysharp.Threading.Tasks;
using DadVSMe.Entities;
using DadVSMe.Entities.FSM;
using H00N.AI.FSM;
using UnityEngine.SceneManagement;

namespace DadVSMe.Players.FSM
{
    public class PlayerDieAction : DieAction
    {
        private const float CHARACTER_ZOOM_BLEND_TIME = 9f;
        private const float FADE_DURATION = 1f;
        private EntityAnimator entityAnimator = null;

        public override void Init(FSMBrain brain, FSMState state)
        {
            base.Init(brain, state);
            entityAnimator = brain.GetComponent<EntityAnimator>();
        }

        public override void EnterState()
        {
            base.EnterState();
            entityAnimator.RemoveAnimationEventListener(EEntityAnimationEventType.End, HandleAnimationEnd);   
            entityAnimator.AddAnimationEventListener(EEntityAnimationEventType.End, HandleAnimationEnd);   

            _ = new ChangeCinemachineCamera(GameInstance.GameCycle.CharacterZoomCinemachineCamera, CHARACTER_ZOOM_BLEND_TIME);
        }

        public override void ExitState()
        {
            base.ExitState();
            entityAnimator.RemoveAnimationEventListener(EEntityAnimationEventType.End, HandleAnimationEnd);   
        }

        private async void HandleAnimationEnd(EntityAnimationEventData eventData)
        {
            TimeManager.SetTimeScale(0f, true);

            await DOFade.FadeInAsync(FADE_DURATION);
            await SceneManager.TryLoadSceneAsync(GameDefine.GAME_OVER_SCENE_NAME, LoadSceneMode.Single);
            DOFade.FadeOutAsync(FADE_DURATION).Forget();

            TimeManager.SetTimeScale(1f, true);
        }
    }
}