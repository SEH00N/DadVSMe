#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DadVSMe.Enemies
{
    [CustomEditor(typeof(EnemySpawnData))]
    public class EnemySpawnDataEditor : Editor
    {
        const float kNarrowThreshold = 420f; // 이보다 좁으면 스택 레이아웃

        private SerializedProperty _totalDurationProp;
        private SerializedProperty _customRangesProp;
        private SerializedProperty _phasesProp;

        // Big button 스타일
        private static GUIStyle _bigBtn;
        private static GUIStyle BigBtn
        {
            get
            {
                if (_bigBtn == null)
                {
                    _bigBtn = new GUIStyle(GUI.skin.button)
                    {
                        fontSize = 12,
                        fixedHeight = 28,
                        alignment = TextAnchor.MiddleCenter
                    };
                }
                return _bigBtn;
            }
        }

        private void OnEnable()
        {
            _totalDurationProp = serializedObject.FindProperty("totalDurationSeconds");
            _customRangesProp = serializedObject.FindProperty("customRanges");
            _phasesProp = serializedObject.FindProperty("enemySawnPhaseList");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // 총 길이
            EditorGUILayout.PropertyField(_totalDurationProp);
            int total = Mathf.Max(1, _totalDurationProp.intValue);

            // Phase와 Range 크기 동기화(Phase를 기준)
            SyncSizes(phasesAsDriver: true);

            // 헤더 (버튼 없음)
            EditorGUILayout.LabelField("Phases", EditorStyles.boldLabel);

            // 요소들
            for (int i = 0; i < _phasesProp.arraySize; i++)
            {
                var phaseProp = _phasesProp.GetArrayElementAtIndex(i);
                var rangeProp = _customRangesProp.GetArrayElementAtIndex(i);
                var startSec = rangeProp.FindPropertyRelative("startSec");
                var endSec = rangeProp.FindPropertyRelative("endSec");

                string header = $"[{i + 1}] {Fmt(startSec.intValue)} ~ {Fmt(endSec.intValue)}  ({Mathf.Max(0, endSec.intValue - startSec.intValue)}s)";

                using (new EditorGUILayout.VerticalScope("box"))
                {
                    // 상단 라벨
                    EditorGUILayout.LabelField(header, EditorStyles.boldLabel);

                    // 범위 편집
                    DrawRangeEditor(startSec, endSec, total);

                    EditorGUILayout.Space(4);

                    // Phase 속성(라벨 안 잘리게)
                    DrawPhaseFields(phaseProp);

                    EditorGUILayout.Space(6);

                    // 하단 큰 버튼들: + / -
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        GUILayout.FlexibleSpace();

                        if (GUILayout.Button("−  Remove", BigBtn, GUILayout.MinWidth(120)))
                        {
                            RemoveAt(i);
                            break;
                        }
                    }
                }
            }

            // 리스트 맨 아래 전역 추가 버튼
            EditorGUILayout.Space(8);
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("+  Add Phase", BigBtn, GUILayout.MinWidth(160)))
                {
                    AddNewAt(_phasesProp.arraySize, total);
                }
            }

            // 유틸
            EditorGUILayout.Space(8);
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Sort & Fix Overlaps"))
                    SortAndFixRanges(total);
                if (GUILayout.Button("Snap Sequential (contiguous)"))
                    SnapSequential(total);
            }

            serializedObject.ApplyModifiedProperties();
        }

        // ---------- 그리기 ----------

        private static void DrawRangeEditor(SerializedProperty startSec, SerializedProperty endSec, int total)
        {
            float s = Mathf.Clamp(startSec.intValue, 0, total);
            float e = Mathf.Clamp(endSec.intValue, 0, total);

            // 슬라이더
            EditorGUILayout.MinMaxSlider(new GUIContent("Seconds"), ref s, ref e, 0f, total);
            startSec.intValue = Mathf.RoundToInt(s);
            endSec.intValue = Mathf.RoundToInt(e);
            if (endSec.intValue < startSec.intValue) endSec.intValue = startSec.intValue;

            // Start / End 편집 행
            DrawMinSecRowResponsive("Start", startSec, total);
            DrawMinSecRowResponsive("End", endSec, total);
        }

        private static void DrawMinSecRowResponsive(string label, SerializedProperty secProp, int total)
        {
            int sec = Mathf.Clamp(secProp.intValue, 0, total);
            int m = sec / 60;
            int s = sec % 60;

            bool stacked = EditorGUIUtility.currentViewWidth < kNarrowThreshold;

            if (!stacked)
            {
                // 가로 2-칼럼 (m | s)
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField(label, GUILayout.Width(48));

                    float oldLW = EditorGUIUtility.labelWidth;
                    EditorGUIUtility.labelWidth = 22f; // 짧은 라벨로 숫자 필드 넓힘

                    // 칼럼 간격과 최소 폭 보장
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        m = EditorGUILayout.IntField(new GUIContent("m"), m, GUILayout.MinWidth(70));
                        GUILayout.Space(8);
                        s = EditorGUILayout.IntField(new GUIContent("s"), s, GUILayout.MinWidth(70));
                    }

                    EditorGUIUtility.labelWidth = oldLW;
                }
            }
            else
            {
                // 세로 스택: 라벨 한 줄 + (m,s) 한 줄
                EditorGUILayout.LabelField(label);
                float oldLW = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = 22f;

                using (new EditorGUILayout.HorizontalScope())
                {
                    m = EditorGUILayout.IntField(new GUIContent("m"), m, GUILayout.MinWidth(70));
                    GUILayout.Space(8);
                    s = EditorGUILayout.IntField(new GUIContent("s"), s, GUILayout.MinWidth(70));
                }

                EditorGUIUtility.labelWidth = oldLW;
            }

            sec = Mathf.Clamp(m * 60 + s, 0, total);
            secProp.intValue = sec;
        }

        private static void DrawPhaseFields(SerializedProperty phaseProp)
        {
            var spawnIntervalProp = phaseProp.FindPropertyRelative("spawnInterval");
            var enemiesDataListProp = phaseProp.FindPropertyRelative("enemiesList");
            var totalEnemyCountProp = phaseProp.FindPropertyRelative("totalEnemyCountOnField");

            // 1) 스폰 주기(RandomRange) — 전용 드로워가 처리
            EditorGUILayout.PropertyField(spawnIntervalProp, new GUIContent("Spawn Interval (Sec)"));

            EditorGUILayout.Space(4);

            // 2) 적 리스트
            EditorGUILayout.PropertyField(enemiesDataListProp, new GUIContent("Spawnable Enemy Data"), includeChildren: true);

            EditorGUILayout.Space(6);

            // 3) 두 줄 스택형
            DrawStackedIntField(totalEnemyCountProp, "Max Enemy Count on Field");
        }

        // ---------- 리스트 조작 ----------

        private void SyncSizes(bool phasesAsDriver)
        {
            int target = phasesAsDriver ? _phasesProp.arraySize : _customRangesProp.arraySize;

            while (_phasesProp.arraySize < target) _phasesProp.InsertArrayElementAtIndex(_phasesProp.arraySize);
            while (_customRangesProp.arraySize < target) _customRangesProp.InsertArrayElementAtIndex(_customRangesProp.arraySize);

            while (_phasesProp.arraySize > target) _phasesProp.DeleteArrayElementAtIndex(_phasesProp.arraySize - 1);
            while (_customRangesProp.arraySize > target) _customRangesProp.DeleteArrayElementAtIndex(_customRangesProp.arraySize - 1);
        }

        private void AddNewAt(int insertIndex, int total)
        {
            insertIndex = Mathf.Clamp(insertIndex, 0, _phasesProp.arraySize);
            _phasesProp.InsertArrayElementAtIndex(insertIndex);
            _customRangesProp.InsertArrayElementAtIndex(insertIndex);

            // 기본 범위: 이전 구간의 end부터 시작
            int start = 0;
            if (insertIndex > 0)
            {
                var prev = _customRangesProp.GetArrayElementAtIndex(insertIndex - 1);
                start = Mathf.Clamp(prev.FindPropertyRelative("endSec").intValue, 0, total);
            }
            int end = Mathf.Clamp(start + Math.Max(1, total / 4), 0, total);

            var seg = _customRangesProp.GetArrayElementAtIndex(insertIndex);
            seg.FindPropertyRelative("startSec").intValue = start;
            seg.FindPropertyRelative("endSec").intValue = end;
        }

        private void RemoveAt(int index)
        {
            if (index < 0 || index >= _phasesProp.arraySize) return;
            _phasesProp.DeleteArrayElementAtIndex(index);
            if (index < _customRangesProp.arraySize)
                _customRangesProp.DeleteArrayElementAtIndex(index);
        }

        private void SortAndFixRanges(int total)
        {
            var temp = new List<(int s, int e)>(_customRangesProp.arraySize);
            for (int i = 0; i < _customRangesProp.arraySize; i++)
            {
                var seg = _customRangesProp.GetArrayElementAtIndex(i);
                int s = Mathf.Clamp(seg.FindPropertyRelative("startSec").intValue, 0, total);
                int e = Mathf.Clamp(seg.FindPropertyRelative("endSec").intValue, 0, total);
                if (e < s) e = s;
                temp.Add((s, e));
            }
            temp.Sort((a, b) => a.s != b.s ? a.s.CompareTo(b.s) : a.e.CompareTo(b.e));

            for (int i = 1; i < temp.Count; i++)
            {
                if (temp[i].s < temp[i - 1].e)
                    temp[i] = (temp[i - 1].e, Math.Max(temp[i - 1].e, temp[i].e));
                temp[i] = (Mathf.Clamp(temp[i].s, 0, total), Mathf.Clamp(temp[i].e, 0, total));
            }

            for (int i = 0; i < _customRangesProp.arraySize; i++)
            {
                var seg = _customRangesProp.GetArrayElementAtIndex(i);
                seg.FindPropertyRelative("startSec").intValue = temp[i].s;
                seg.FindPropertyRelative("endSec").intValue = temp[i].e;
            }
        }

        private void SnapSequential(int total)
        {
            SortAndFixRanges(total);
            if (_customRangesProp.arraySize == 0) return;

            int cursor = Mathf.Clamp(_customRangesProp.GetArrayElementAtIndex(0).FindPropertyRelative("startSec").intValue, 0, total);
            for (int i = 0; i < _customRangesProp.arraySize; i++)
            {
                var seg = _customRangesProp.GetArrayElementAtIndex(i);
                int s = Mathf.Clamp(seg.FindPropertyRelative("startSec").intValue, 0, total);
                int e = Mathf.Clamp(seg.FindPropertyRelative("endSec").intValue, 0, total);

                s = cursor;
                if (e < s) e = s;

                seg.FindPropertyRelative("startSec").intValue = s;
                seg.FindPropertyRelative("endSec").intValue = e;

                cursor = e;
            }
        }

        // ---------- Misc ----------

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

        private static void DrawStackedIntField(SerializedProperty prop, string label)
        {
            // 충분한 높이 예약(라벨H + 필드H + 여백)
            var style = new GUIStyle(EditorStyles.label) { wordWrap = true };
            float labelH = style.CalcHeight(new GUIContent(label), EditorGUIUtility.currentViewWidth - 40f);
            float height = labelH + EditorGUIUtility.singleLineHeight + 6f;

            var rect = EditorGUILayout.GetControlRect(hasLabel: false, height: height);
            var labelRect = new Rect(rect.x, rect.y, rect.width, labelH);
            var fieldRect = new Rect(rect.x, rect.y + labelH + 2, rect.width, EditorGUIUtility.singleLineHeight);

            EditorGUI.LabelField(labelRect, label, style);
            prop.intValue = EditorGUI.IntField(fieldRect, GUIContent.none, prop.intValue);
        }
    }
}
#endif
