using Cysharp.Threading.Tasks;
using DadVSMe.Entities;
using DadVSMe.Entities.FSM;
using H00N.AI.FSM;
using UnityEngine.SceneManagement;

namespace DadVSMe.Players.FSM
{
    public class PlayerDieAction : DieAction
    {
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
        }

        public override void ExitState()
        {
            base.ExitState();
            entityAnimator.RemoveAnimationEventListener(EEntityAnimationEventType.End, HandleAnimationEnd);   
        }

        private void HandleAnimationEnd(EntityAnimationEventData eventData)
        {
            SceneManager.TryLoadSceneAsync(GameDefine.GAME_OVER_SCENE_NAME, LoadSceneMode.Single).Forget();
        }
    }
}