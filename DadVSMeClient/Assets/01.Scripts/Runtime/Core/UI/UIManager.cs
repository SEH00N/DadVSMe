using UnityEngine;

namespace DadVSMe.Core.UI
{
    public abstract class UIHandle<T> where T : UIHandleParameter
    {
        public abstract void Execute(T handleParameter);
    }

    public class UIHandleParameter
    {
        public Canvas MainCanvas { get; internal set; }
        public UIHandleParameter() {} 
    }

    public static class UIManager
    {
        private static Canvas mainCanvas;
        public static Canvas MainCanvas
        {
            get
            {
                if (mainCanvas == null)
                {
                    mainCanvas = GameObject.Find("MainCanvas").GetComponent<Canvas>();
                }

                return mainCanvas;
            }
        }

        public static THandle CreateUIHandle<THandle, TParam>(out TParam param)
            where TParam : UIHandleParameter, new()
            where THandle : UIHandle<TParam>, new()
        {
            var handle = new THandle();
            param = CreateUIHandleParameter<TParam>();
            return handle;
        }

        public static TParam CreateUIHandleParameter<TParam>()
            where TParam : UIHandleParameter, new()
        {
            var p = new TParam();
            p.MainCanvas = MainCanvas; // 매니저만 주입
            return p;
        }
    }
}
