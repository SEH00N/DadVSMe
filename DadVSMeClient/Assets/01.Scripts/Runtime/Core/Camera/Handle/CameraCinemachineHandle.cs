using Unity.Cinemachine;

namespace DadVSMe.Core.Cam
{
    public class CameraCinemachineHandle : CameraHandle<CameraCinemachineHandle.Parameter>
    {
        public class Parameter : CameraHandleParameter
        {
            public CinemachineCamera cinemachineCamera;

            public Parameter() { }
        }

        private const int ACTIVE_PRIORITY = 1000;
        private const int INACTIVE_PRIORITY = 0;

        private static CinemachineCamera activeedCinemachineCamera = null;

        public override void Execute(Parameter handleParameter)
        {
            if(activeedCinemachineCamera != null)
                activeedCinemachineCamera.Priority = INACTIVE_PRIORITY;

            activeedCinemachineCamera = handleParameter.cinemachineCamera;
            activeedCinemachineCamera.Priority = ACTIVE_PRIORITY;
        }
    }

}
