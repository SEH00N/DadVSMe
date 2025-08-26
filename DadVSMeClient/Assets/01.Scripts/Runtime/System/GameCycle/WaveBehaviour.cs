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
        protected CancellationToken CancellationToken => cancellationTokenSource?.Token ?? CancellationToken.None;

        protected virtual void OnEnable()
        {
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
                cancellationTokenSource.Dispose();
                cancellationTokenSource = null;
            }

            cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(destroyCancellationToken);

            UpdateLoop();
        }

        protected virtual void OnDisable()
        {
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
                cancellationTokenSource.Dispose();
                cancellationTokenSource = null;
            }
        }

        protected abstract bool OnUpdate();
        private async void UpdateLoop()
        {
            try {
                while (CancellationToken != CancellationToken.None && CancellationToken.IsCancellationRequested == false)
                {
                    bool isEnd = OnUpdate();
                    if(isEnd)
                        break;

                    await UniTask.Delay(TimeSpan.FromSeconds(UDPATE_INTERVAL), cancellationToken: CancellationToken);
                }
            }
            catch (OperationCanceledException) { }
            finally
            {
                cancellationTokenSource?.Dispose();
                cancellationTokenSource = null;
            }
        }
    }
}