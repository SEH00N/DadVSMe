using UnityEngine;

namespace DadVSMe.Core.Cam
{
    public abstract class CameraHandle<T> where T : CameraHandleParameter
    {
        public abstract void Execute(T handleParameter);
    }

    public class CameraHandleParameter
    {
        public Camera Cam { get; internal set; }
        public CameraHandleParameter() {} 
    }

    public static class CameraManager
    {
        private static Camera mainCam;
        public static Camera MainCam
        {
            get
            {
                if (mainCam == null)
                {
                    mainCam = Camera.main;
                }

                return mainCam;
            }
        }

        public static THandle CreateCameraHandle<THandle, TParam>(out TParam param)
            where TParam : CameraHandleParameter, new()
            where THandle : CameraHandle<TParam>, new()
        {
            var handle = new THandle();
            param = CreateCameraHandleParameter<TParam>();
            return handle;
        }

        public static TParam CreateCameraHandleParameter<TParam>()
            where TParam : CameraHandleParameter, new()
        {
            var p = new TParam();
            p.Cam = MainCam; // 매니저만 주입
            return p;
        }
    }
}
