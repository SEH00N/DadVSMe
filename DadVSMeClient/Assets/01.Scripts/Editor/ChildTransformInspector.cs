using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DadVSMe.Editors
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Transform))]
    public class ChildTransformInspector : Editor
    {
        private Editor _defaultInspector;

        private void OnEnable()
        {
            // Create an instance of Unity's internal default Transform inspector via reflection
            var defaultType = Type.GetType("UnityEditor.TransformInspector, UnityEditor");
            if (defaultType != null)
            {
                _defaultInspector = CreateEditor(targets, defaultType);
            }
        }

        private void OnDisable()
        {
            if (_defaultInspector != null)
            {
                DestroyImmediate(_defaultInspector);
                _defaultInspector = null;
            }
        }

        public override void OnInspectorGUI()
        {
            // Draw Unity's default Transform inspector first
            if (_defaultInspector != null)
            {
                _defaultInspector.OnInspectorGUI();
            }
            else
            {
                DrawFallbackTransform();
            }

            // Show extra fields only when at least one selected Transform is a child
            bool anyChild = targets.Any(o => ((Transform)o).parent != null);
            if (!anyChild) return;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("World Transform", EditorStyles.boldLabel);
            DrawWorldLocalPositionFields();
        }

        private void DrawWorldLocalPositionFields()
        {
            var first = (Transform)targets[0];
            Vector3 world = first.position;
            Vector3 local = first.localPosition;

            bool mixedWorld = false;
            bool mixedLocal = false;

            for (int i = 1; i < targets.Length; i++)
            {
                var t = (Transform)targets[i];
                if (t.position != world) mixedWorld = true;
                if (t.localPosition != local) mixedLocal = true;
            }

            // World Position field
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = mixedWorld;
            Vector3 newWorld = EditorGUILayout.Vector3Field("World Position", world);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObjects(targets, "Change World Position");
                foreach (var obj in targets)
                {
                    var t = (Transform)obj;
                    t.position = newWorld;
                    EditorUtility.SetDirty(t);
                }
            }

            // Local Position field
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = mixedLocal;
        }

        private void DrawFallbackTransform()
        {
            var tr = (Transform)target;
            EditorGUI.BeginChangeCheck();
            Vector3 pos = EditorGUILayout.Vector3Field("Position", tr.localPosition);
            Vector3 rot = EditorGUILayout.Vector3Field("Rotation", tr.localEulerAngles);
            Vector3 scl = EditorGUILayout.Vector3Field("Scale", tr.localScale);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(tr, "Change Transform");
                tr.localPosition = pos;
                tr.localEulerAngles = rot;
                tr.localScale = scl;
                EditorUtility.SetDirty(tr);
            }
        }
    }
}
