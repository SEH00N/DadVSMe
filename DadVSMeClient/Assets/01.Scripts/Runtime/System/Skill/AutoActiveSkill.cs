using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DadVSMe
{
    public class AutoActiveSkill : UnitSkill
    {
        protected float cooltime;
        private CancellationTokenSource _loopCts;

        public AutoActiveSkill(float cooltime) : base()
        {
            this.cooltime = cooltime;
        }

        public override void OnRegist(UnitSkillComponent ownerComponent)
        {
            base.OnRegist(ownerComponent);

            StartAutoActiveLoop();
        }

        public override void Execute()
        {

        }

        public override void OnUnregist()
        {
            base.OnUnregist();

            StopAutoActiveLoop();
        }

        public void StartAutoActiveLoop()
    {
        // 중복 실행 방지
        _loopCts?.Cancel();
        _loopCts?.Dispose();
        _loopCts = CancellationTokenSource.CreateLinkedTokenSource(ownerComponent.destroyCancellationToken);

        // 필요하면 파괴 시에도 끊고 싶을 때는 Destroy 토큰과 링크:
        // _loopCts = CancellationTokenSource.CreateLinkedTokenSource(this.GetCancellationTokenOnDestroy());

        AutoActiveLoop(_loopCts.Token).Forget();
    }

    public void StopAutoActiveLoop()
    {
        _loopCts?.Cancel();
        _loopCts?.Dispose();
        _loopCts = null;
    }

    async UniTask AutoActiveLoop(CancellationToken ct)
    {
        float elapsed = 0f;

        while (!ct.IsCancellationRequested)
        {
            await UniTask.Yield(PlayerLoopTiming.Update, ct); // Fixed면 FixedUpdate로
            elapsed += Time.deltaTime;                        // 타임스케일 무시: unscaledDeltaTime

            if (elapsed >= cooltime)
            {
                Execute();
                
                elapsed = 0f;
            }
        }
    }
    }
}
