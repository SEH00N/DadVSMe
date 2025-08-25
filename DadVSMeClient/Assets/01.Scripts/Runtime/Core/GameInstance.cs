using DadVSMe.GameCycles;
using UnityEngine;
using UnityEngine.UI;

namespace DadVSMe
{
    public static class GameInstance
    {
        private static Transform mainPopupFrame = null;
        public static Transform MainPopupFrame {
            get {
                if(mainPopupFrame == null)
                {
                    GameObject mainPopupFrameObject = GameObject.Find("MainPopupFrame");
                    if(mainPopupFrameObject != null)
                        mainPopupFrame = mainPopupFrameObject.transform;
                }

                return mainPopupFrame;
            }
        }

        private static GameCycle gameCycle = null;
        public static GameCycle GameCycle {
            get {
                if(gameCycle == null)
                    gameCycle = Object.FindFirstObjectByType<GameCycle>();

                return gameCycle;
            }
        }

        private static Transform staticCanvas = null;
        public static Transform StaticCanvas {
            get {
                if(staticCanvas == null)
                {
                    GameObject staticCanvasObject = GameObject.Find("StaticCanvas");
                    if(staticCanvasObject != null)
                        staticCanvas = staticCanvasObject.transform;
                }

                return staticCanvas;
            }
        }

        private static Image fadeImage = null;
        public static Image FadeImage {
            get {
                if(fadeImage == null)
                {
                    if(StaticCanvas == null)
                        return null;
                    
                    Transform fadeImageTransform = StaticCanvas.Find("FadeImage");
                    if(fadeImageTransform != null)
                        fadeImage = fadeImageTransform.GetComponent<Image>();
                }

                return fadeImage;
            }
        }
    }
}