using DadVSMe.Enemies;
using UnityEditor;
using UnityEngine;

namespace DadVSMe.Editors
{
    [CustomEditor(typeof(KidEnemyData), true)]
    public class KidEnemyDataEditor : Editor
    {
        private SerializedProperty hatSpriteProperty;
        private SerializedProperty clothesSpriteProperty;

        private void OnEnable()
        {
            // SerializedProperty를 사용하여 특정 필드를 찾습니다.
            hatSpriteProperty = serializedObject.FindProperty("hatSprite");
            clothesSpriteProperty = serializedObject.FindProperty("clothesSprite");
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.LabelField("Hat Sprite");
            hatSpriteProperty.objectReferenceValue = EditorGUILayout.ObjectField(hatSpriteProperty.objectReferenceValue, typeof(Sprite), true, GUILayout.Height(60), GUILayout.Width(60)) as Sprite;
            EditorGUILayout.LabelField("Clothes Sprite");
            clothesSpriteProperty.objectReferenceValue = EditorGUILayout.ObjectField(clothesSpriteProperty.objectReferenceValue, typeof(Sprite), true, GUILayout.Height(60), GUILayout.Width(60)) as Sprite;

            SerializedProperty iterator = serializedObject.GetIterator();
            bool enterChildren = true;
            while (iterator.NextVisible(enterChildren))
            {
                enterChildren = false;
                if (iterator.name != "hatSprite" && iterator.name != "clothesSprite" && iterator.name != "m_Script")
                    EditorGUILayout.PropertyField(iterator, true);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}