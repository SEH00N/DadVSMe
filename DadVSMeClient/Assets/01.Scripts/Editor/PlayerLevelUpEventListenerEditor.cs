using UnityEngine;
using UnityEditor;
using DadVSMe.Players;

namespace DadVSMe.Editors
{
    [CustomEditor(typeof(PlayerLevelUpEventListener))]
    public class PlayerLevelUpEventListenerEditor : Editor
    {
        public override void OnInspectorGUI()
        {        
            DrawDefaultInspector();

            EditorGUILayout.Space(10f);

            if (GUILayout.Button("LevelUp"))
            {
                if(EditorApplication.isPlaying == false)
                    return;

                (target as PlayerLevelUpEventListener).OnLevelUp(0);
            }
        }
    }
}
