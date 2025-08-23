using DadVSMe.Players;
using UnityEngine;

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
    }
}