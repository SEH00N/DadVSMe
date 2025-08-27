using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DadVSMe.GameCycles
{
    public abstract class WaveBehaviour : MonoBehaviour
    {
        private const float UDPATE_INTERVAL = 0.5f;

        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        protected virtual void PreOnEnable() { }
        protected virtual void PostOnEnable() { }
        private void OnEnable()
        {
            PreOnEnable();

            StartUpdateLoop();
            
            PostOnEnable();
        }

        protected virtual void PreOnDisable() { }
        protected virtual void PostOnDisable() { }
        private void OnDisable()
        {
            PreOnDisable();

            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
                cancellationTokenSource.Dispose();
                cancellationTokenSource = null;
            }

            PostOnDisable();
        }

        private async void StartUpdateLoop()
        {
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
                cancellationTokenSource.Dispose();
                cancellationTokenSource = null;
            }

            cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(destroyCancellationToken);

            await UpdateLoop(cancellationTokenSource.Token);

            cancellationTokenSource?.Dispose();
            cancellationTokenSource = null;
        }

        protected abstract bool OnUpdate();
        private async UniTask UpdateLoop(CancellationToken cancellationToken)
        {
            try {
                while (cancellationToken.IsCancellationRequested == false)
                {
                    bool isEnd = OnUpdate();
                    if(isEnd)
                        break;

                    await UniTask.Delay(TimeSpan.FromSeconds(UDPATE_INTERVAL), cancellationToken: cancellationToken);
                }
            }
            catch (OperationCanceledException) { }
        }
    }
}