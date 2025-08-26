using Unity.Cinemachine;

namespace DadVSMe.Core.Cam
{
    public class CameraCinemachineHandle : CameraHandle<CameraCinemachineHandle.Parameter>
    {
        public class Parameter : CameraHandleParameter
        {
            public CinemachineCamera cinemachineCamera;
            public float blendTime = 1f;

            public Parameter() { }
        }

        private const int ACTIVE_PRIORITY = 1000;
        private const int INACTIVE_PRIORITY = 0;

        private static CinemachineCamera activedCinemachineCamera = null;

        public override void ExecuteAsync(Parameter handleParameter)
        {
            if(activedCinemachineCamera != null)
                activedCinemachineCamera.Priority = INACTIVE_PRIORITY;

            activedCinemachineCamera = handleParameter.cinemachineCamera;
            activedCinemachineCamera.Priority = ACTIVE_PRIORITY;

            handleParameter.CinemachineBrain.DefaultBlend.Time = handleParameter.blendTime;
        }
    }

}
