using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace DadVSMe
{
    public abstract class AutoActiveSkill<TData, TOption> : UnitSkill<TData, TOption> where TData : SkillDataBase where TOption : SkillOption
    {
        private CancellationTokenSource _loopCts;

        public override void OnRegist(UnitSkillComponent ownerComponent, SkillDataBase skillData)
        {
            base.OnRegist(ownerComponent, skillData);

            StartAutoActiveLoop();
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

        protected abstract float GetCoolTime();
        private async UniTask AutoActiveLoop(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(GetCoolTime()), cancellationToken: ct);
                Execute();
            }
        }
    }
}
