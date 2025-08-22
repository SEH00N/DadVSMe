using DadVSMe.Players;
using UnityEngine;

namespace DadVSMe
{
    public static class GameInstance
    {
        private static Player mainPlayer = null;
        public static Player MainPlayer {
            get {
                if(mainPlayer == null)
                    mainPlayer = Object.FindFirstObjectByType<Player>();

                return mainPlayer;
            }
        }

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
    }
}