using Cysharp.Threading.Tasks;
using Unity.Cinemachine;

namespace DadVSMe.Core.Cam
{
    public class CameraShakeHandle : CameraHandle<CameraShakeHandle.Parameter>
    {
        public class Parameter : CameraHandleParameter
        {
            public CinemachineCamera cinemachineCamera;
            public float duration = 1f;
            public float amplitude = 1f;
            public float frequency = 1f;

            public Parameter() { }
        }

        public override async void ExecuteAsync(Parameter handleParameter)
        {
            CinemachineBasicMultiChannelPerlin perlin = handleParameter.cinemachineCamera.GetCinemachineComponent(CinemachineCore.Stage.Noise) as CinemachineBasicMultiChannelPerlin;
            perlin.AmplitudeGain = handleParameter.amplitude;
            perlin.FrequencyGain = handleParameter.frequency;

            await UniTask.WaitForSeconds(handleParameter.duration, ignoreTimeScale: true);

            perlin.AmplitudeGain = 0f;
            perlin.FrequencyGain = 0f;
        }
    }

}
