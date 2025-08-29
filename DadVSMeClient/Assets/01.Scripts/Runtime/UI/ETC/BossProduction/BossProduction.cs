using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DadVSMe.UI
{
    [RequireComponent(typeof(AnimationUI), typeof(Animator))]
    public class BossProduction : PoolableBehaviourUI
    {
        [SerializeField] Animator _animator;
        private bool _playingProductionFlag = false;

        public async UniTask PlayProductionAsync()
        {
            StretchRect();

            _playingProductionFlag = false;

            _animator.updateMode = AnimatorUpdateMode.UnscaledTime;

            await UniTask.WaitWhile(()=> _playingProductionFlag);
        }

        public void CompletePlayingProduction()
        {
            _playingProductionFlag = true;
        }
    }
}
