#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;

namespace DadVSMe
{
    [CustomPropertyDrawer(typeof(RandomRangeFloat))]
    public class RandomRangeFloatDrawer : PropertyDrawer
    {
        const float kMinLimit = 0.01f;
        const float kMaxLimit = 60f;
        const float kColsGap = 12f;
        const float kStackThreshold = 360f;

        // 0.01 단위 반올림
        static float Round2(float v) => (float)Math.Round(v, 2, MidpointRounding.AwayFromZero);

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var minProp = property.FindPropertyRelative("min");
            var maxProp = property.FindPropertyRelative("max");

            float min = Mathf.Max(kMinLimit, minProp.floatValue <= 0f ? kMinLimit : minProp.floatValue);
            float max = Mathf.Max(kMinLimit, maxProp.floatValue <= 0f ? kMinLimit : maxProp.floatValue);
            if (max < min) max = min;

            float lineH = EditorGUIUtility.singleLineHeight;
            float vsp = EditorGUIUtility.standardVerticalSpacing;

            // 라벨
            var labelRect = new Rect(position.x, position.y, position.width, lineH);
            EditorGUI.LabelField(labelRect, label);

            // 슬라이더
            var sliderRect = new Rect(position.x, position.y + lineH + vsp, position.width, lineH);
            EditorGUI.MinMaxSlider(sliderRect, ref min, ref max, kMinLimit, kMaxLimit);

            // 슬라이더 값도 0.01 단위로 정규화
            min = Round2(min);
            max = Round2(max);

            bool stacked = position.width < kStackThreshold;

            if (!stacked)
            {
                var fieldsRect = new Rect(position.x, sliderRect.y + lineH + vsp, position.width, lineH);
                float colW = (fieldsRect.width - kColsGap) * 0.5f;

                var minRect = new Rect(fieldsRect.x, fieldsRect.y, colW, lineH);
                var maxRect = new Rect(fieldsRect.x + colW + kColsGap, fieldsRect.y, colW, lineH);

                float oldLW = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = 36f;
                min = Round2(EditorGUI.FloatField(minRect, new GUIContent("Min"), min));
                max = Round2(EditorGUI.FloatField(maxRect, new GUIContent("Max"), max));
                EditorGUIUtility.labelWidth = oldLW;
            }
            else
            {
                var minLine = new Rect(position.x, sliderRect.y + lineH + vsp, position.width, lineH);
                var maxLine = new Rect(position.x, minLine.y + lineH + vsp, position.width, lineH);

                float oldLW = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = 36f;
                min = Round2(EditorGUI.FloatField(minLine, new GUIContent("Min"), min));
                max = Round2(EditorGUI.FloatField(maxLine, new GUIContent("Max"), max));
                EditorGUIUtility.labelWidth = oldLW;
            }

            if (max < min) max = min;
            min = Mathf.Clamp(min, kMinLimit, kMaxLimit);
            max = Mathf.Clamp(max, kMinLimit, kMaxLimit);

            // 최종적으로 0.01 단위로 저장
            minProp.floatValue = Round2(min);
            maxProp.floatValue = Round2(max);

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float lineH = EditorGUIUtility.singleLineHeight;
            float vsp = EditorGUIUtility.standardVerticalSpacing;
            float wide = lineH * 3f + vsp * 2f;
            float narrow = lineH * 4f + vsp * 3f;
            return Mathf.Max(wide, narrow);
        }
    }
}
#endif
