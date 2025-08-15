using Cysharp.Threading.Tasks;
using H00N.AI.FSM;
using System;
using System.Threading;
using UnityEngine;

namespace DadVSMe.Entities.FSM
{
    public class LieAction : FSMAction
    {
        [SerializeField] float idleTime = 1f;
        [SerializeField] FSMState onGoingState = null;

        private CancellationTokenSource cancellationTokenSource = null;

        public async override void EnterState()
        {
            base.EnterState();

            try {
                cancellationTokenSource?.Cancel();
                cancellationTokenSource?.Dispose();
                cancellationTokenSource = new CancellationTokenSource();

                await UniTask.Delay((int)(idleTime * 1000), cancellationToken: cancellationTokenSource.Token);
                
                cancellationTokenSource?.Dispose();
                cancellationTokenSource = null;

                brain.ChangeState(onGoingState);
            }
            catch(OperationCanceledException) { }
            finally {
                cancellationTokenSource?.Dispose();
                cancellationTokenSource = null;
            }
        }

        public override void ExitState()
        {
            base.ExitState();

            cancellationTokenSource?.Cancel();
            cancellationTokenSource?.Dispose();
            cancellationTokenSource = null;
        }
    }
}
