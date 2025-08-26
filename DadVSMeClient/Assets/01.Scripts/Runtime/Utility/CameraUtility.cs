using DadVSMe.Core.Cam;
using Unity.Cinemachine;

namespace DadVSMe
{
    public struct ChangeCinemachineCamera
    {
        public ChangeCinemachineCamera(CinemachineCamera cinemachineCamera)
        {
            CameraCinemachineHandle cameraCinemachineHandle = CameraManager.CreateCameraHandle<CameraCinemachineHandle, CameraCinemachineHandle.Parameter>(out CameraCinemachineHandle.Parameter cameraCinemachineHandleParam);
            cameraCinemachineHandleParam.cinemachineCamera = cinemachineCamera;
            cameraCinemachineHandle.Execute(cameraCinemachineHandleParam);
        }
    }
}
