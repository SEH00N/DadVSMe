#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DadVSMe
{
    [CustomEditor(typeof(EnemySpawnData))]
    public class EnemySpawnDataEditor : Editor
    {
        private SerializedProperty _divedCountProp;
        private SerializedProperty _totalDurationProp;
        private SerializedProperty _useCustomRangesProp;
        private SerializedProperty _customRangesProp;
        private SerializedProperty _phasesProp;

        private void OnEnable()
        {
            _divedCountProp = serializedObject.FindProperty("divedCount");
            _totalDurationProp = serializedObject.FindProperty("totalDurationSeconds");
            _useCustomRangesProp = serializedObject.FindProperty("useCustomRanges");
            _customRangesProp = serializedObject.FindProperty("customRanges");
            _phasesProp = serializedObject.FindProperty("enemySawnPhaseList");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // Basic controls
            EditorGUILayout.PropertyField(_divedCountProp);
            EditorGUILayout.PropertyField(_totalDurationProp);
            EditorGUILayout.PropertyField(_useCustomRangesProp);

            int count = Mathf.Max(1, _divedCountProp.intValue);
            int total = Mathf.Max(1, _totalDurationProp.intValue);

            // Keep sizes in sync
            SyncListSize(_phasesProp, count);
            SyncListSize(_customRangesProp, count);

            // Equal-division segments (computed, not stored)
            var segments = _useCustomRangesProp.boolValue
                ? ReadCustomSegments(_customRangesProp, total) // editable by user
                : ComputeRoundedSegments(count, total);        // auto

            // When using custom ranges, show editors for each segment
            if (_useCustomRangesProp.boolValue)
            {
                EditorGUILayout.Space(6);
                EditorGUILayout.LabelField("Custom Time Ranges", EditorStyles.boldLabel);

                for (int i = 0; i < count; i++)
                {
                    var segProp = _customRangesProp.GetArrayElementAtIndex(i);
                    var startSec = segProp.FindPropertyRelative("startSec");
                    var endSec = segProp.FindPropertyRelative("endSec");

                    using (new EditorGUILayout.VerticalScope("box"))
                    {
                        EditorGUILayout.LabelField($"[{i + 1}] Range", EditorStyles.boldLabel);

                        // MinMax slider (seconds)
                        float s = Mathf.Clamp(startSec.intValue, 0, total);
                        float e = Mathf.Clamp(endSec.intValue, 0, total);
                        EditorGUILayout.MinMaxSlider(new GUIContent("Seconds"), ref s, ref e, 0f, total);
                        startSec.intValue = Mathf.RoundToInt(s);
                        endSec.intValue = Mathf.RoundToInt(e);

                        // Minutes/Seconds fields for precision
                        DrawMinSecRow("Start", startSec, total);
                        DrawMinSecRow("End", endSec, total);

                        // Fix ordering if needed
                        if (endSec.intValue < startSec.intValue)
                            endSec.intValue = startSec.intValue;

                        EditorGUILayout.LabelField($"¡æ {Fmt(startSec.intValue)} ~ {Fmt(endSec.intValue)}  ({Mathf.Max(0, endSec.intValue - startSec.intValue)}s)");
                    }
                }

                // Utilities
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Auto-fill equal divisions"))
                    {
                        var eq = ComputeRoundedSegments(count, total);
                        for (int i = 0; i < count; i++)
                        {
                            var segProp = _customRangesProp.GetArrayElementAtIndex(i);
                            segProp.FindPropertyRelative("startSec").intValue = eq[i].start;
                            segProp.FindPropertyRelative("endSec").intValue = eq[i].end;
                        }
                    }
                    if (GUILayout.Button("Sort & Fix Overlaps"))
                    {
                        SortAndFix(_customRangesProp, total);
                    }
                }
            }

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Phases", EditorStyles.boldLabel);

            // Draw phases with headers showing the resolved segment labels
            for (int i = 0; i < count; i++)
            {
                var p = _phasesProp.GetArrayElementAtIndex(i);
                var label = $"[{i + 1}] {Fmt(segments[i].start)} ~ {Fmt(segments[i].end)}  ({Mathf.Max(0, segments[i].end - segments[i].start)}s)";

                using (new EditorGUILayout.VerticalScope("box"))
                {
                    EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(p, includeChildren: true);
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        // Helpers

        private static void SyncListSize(SerializedProperty listProp, int targetSize)
        {
            if (listProp == null) return;
            while (listProp.arraySize < targetSize) listProp.InsertArrayElementAtIndex(listProp.arraySize);
            while (listProp.arraySize > targetSize) listProp.DeleteArrayElementAtIndex(listProp.arraySize - 1);
        }

        private static List<(int start, int end)> ReadCustomSegments(SerializedProperty customRangesProp, int total)
        {
            var res = new List<(int, int)>(customRangesProp.arraySize);
            for (int i = 0; i < customRangesProp.arraySize; i++)
            {
                var seg = customRangesProp.GetArrayElementAtIndex(i);
                int s = Mathf.Clamp(seg.FindPropertyRelative("startSec").intValue, 0, total);
                int e = Mathf.Clamp(seg.FindPropertyRelative("endSec").intValue, 0, total);
                if (e < s) e = s;
                res.Add((s, e));
            }
            return res;
        }

        private static List<(int start, int end)> ComputeRoundedSegments(int n, int totalSeconds)
        {
            var result = new List<(int, int)>(n);
            if (n <= 0) { result.Add((0, totalSeconds)); return result; }

            var bounds = new int[n + 1];
            for (int i = 0; i <= n; i++)
            {
                double raw = (double)i * totalSeconds / n;
                bounds[i] = (int)Math.Round(raw, MidpointRounding.AwayFromZero);
            }
            bounds[0] = 0;
            bounds[n] = totalSeconds;

            for (int i = 1; i <= n; i++)
            {
                if (bounds[i] <= bounds[i - 1])
                    bounds[i] = Math.Min(totalSeconds, bounds[i - 1] + 1);
            }
            bounds[n] = totalSeconds;

            for (int i = 0; i < n; i++)
                result.Add((bounds[i], bounds[i + 1]));

            return result;
        }

        private static void SortAndFix(SerializedProperty customRangesProp, int total)
        {
            // Load to list
            var temp = new List<(int s, int e)>();
            for (int i = 0; i < customRangesProp.arraySize; i++)
            {
                var seg = customRangesProp.GetArrayElementAtIndex(i);
                int s = Mathf.Clamp(seg.FindPropertyRelative("startSec").intValue, 0, total);
                int e = Mathf.Clamp(seg.FindPropertyRelative("endSec").intValue, 0, total);
                if (e < s) e = s;
                temp.Add((s, e));
            }

            // Sort by start then end
            temp.Sort((a, b) => a.s != b.s ? a.s.CompareTo(b.s) : a.e.CompareTo(b.e));

            // Make non-overlapping and monotonic increasing
            for (int i = 1; i < temp.Count; i++)
            {
                if (temp[i].s < temp[i - 1].e)
                    temp[i] = (temp[i - 1].e, Math.Max(temp[i - 1].e, temp[i].e));
                temp[i] = (Mathf.Clamp(temp[i].s, 0, total), Mathf.Clamp(temp[i].e, 0, total));
            }

            // Write back
            for (int i = 0; i < customRangesProp.arraySize; i++)
            {
                var seg = customRangesProp.GetArrayElementAtIndex(i);
                seg.FindPropertyRelative("startSec").intValue = temp[i].s;
                seg.FindPropertyRelative("endSec").intValue = temp[i].e;
            }
        }

        private static string Fmt(int sec)
        {
            if (sec < 0) sec = 0;

            int h = sec / 3600;
            int m = (sec % 3600) / 60;
            int s = sec % 60;

            return h > 0 ? $"{h:D2}:{m:D2}:{s:D2}" : $"{m:D2}:{s:D2}";
        }

        private static void DrawMinSecRow(string label, SerializedProperty secProp, int total)
        {
            int sec = Mathf.Clamp(secProp.intValue, 0, total);
            int m = sec / 60;
            int s = sec % 60;

            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField(label, GUILayout.Width(46));
                m = EditorGUILayout.IntField("Min", m);
                s = EditorGUILayout.IntField("Sec", s);
            }

            sec = Mathf.Clamp(m * 60 + s, 0, total);
            secProp.intValue = sec;
        }
    }
}
#endif
