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
            cameraCinemachineHandle.Execute(cameraCinemachineHandleParam);
        }
    }
}
