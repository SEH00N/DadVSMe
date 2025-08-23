using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DadVSMe.Core.Cam
{
    public class BackgroundFilterCameraHandle : CameraHandle<BackgroundFilterCameraHandleParameter>
    {
        public override async void Execute(BackgroundFilterCameraHandleParameter handleParameter)
        {
            SpriteRenderer backgroundFilterRenderer = handleParameter.Cam.GetComponentInChildren<SpriteRenderer>();
            Color temp = backgroundFilterRenderer.color;

            backgroundFilterRenderer.enabled = true;
            backgroundFilterRenderer.color = handleParameter.color;

            await UniTask.WaitForSeconds(handleParameter.time);

            backgroundFilterRenderer.color = temp;
            backgroundFilterRenderer.enabled = false;
        }
    }

    public class BackgroundFilterCameraHandleParameter : CameraHandleParameter
    {
        public float time;
        public Color color;

        public BackgroundFilterCameraHandleParameter() { }
    }
}
