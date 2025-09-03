using UnityEngine;
using UnityEditor;
using DadVSMe.GameCycles;

namespace DadVSMe.Editors
{
    [CustomEditor(typeof(GameCycle))]
    public class GameCycleEditor : Editor
    {
        public override void OnInspectorGUI()
        {        
            DrawDefaultInspector();

            EditorGUILayout.Space(10f);

            string buttonText = GameSettings.Editor.SHOULD_PLAY_GAME_START_DIRECTING ? "Disable Game Start Directing" : "Enable Game Start Directing";
            if (GUILayout.Button(buttonText))
                GameSettings.Editor.SHOULD_PLAY_GAME_START_DIRECTING = !GameSettings.Editor.SHOULD_PLAY_GAME_START_DIRECTING;
        }
    }
}
