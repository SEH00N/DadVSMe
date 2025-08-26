using DadVSMe.Core.Cam;
using Unity.Cinemachine;

namespace DadVSMe
{
    public struct ChangeCinemachineCamera
    {
        public ChangeCinemachineCamera(CinemachineCamera cinemachineCamera, float blendTime = 1f)
        {
            CameraCinemachineHandle cameraCinemachineHandle = CameraManager.CreateCameraHandle<CameraCinemachineHandle, CameraCinemachineHandle.Parameter>(out CameraCinemachineHandle.Parameter cameraCinemachineHandleParam);
            cameraCinemachineHandleParam.cinemachineCamera = cinemachineCamera;
            cameraCinemachineHandleParam.blendTime = blendTime;
            cameraCinemachineHandle.ExecuteAsync(cameraCinemachineHandleParam);
        }
    }

    public struct ShakeCamera
    {
        public ShakeCamera(CinemachineCamera cinemachineCamera, float duration, float amplitude, float frequency)
        {
            CameraShakeHandle cameraShakeHandle = CameraManager.CreateCameraHandle<CameraShakeHandle, CameraShakeHandle.Parameter>(out CameraShakeHandle.Parameter cameraShakeHandleParam);
            cameraShakeHandleParam.cinemachineCamera = cinemachineCamera;
            cameraShakeHandleParam.duration = duration;
            cameraShakeHandleParam.amplitude = amplitude;
            cameraShakeHandleParam.frequency = frequency;
            cameraShakeHandle.ExecuteAsync(cameraShakeHandleParam);
        }
    }
}
