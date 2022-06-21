using Demo.Com;
using LCToolkit;
using LCToolkit.Core;
using UnityEditor;
using UnityEngine;

namespace Demo
{
    [CustomObjectDrawer(typeof(PropertyInfo))]
    public class PropertyInfoDrawer : ObjectDrawer
    {
        public override object OnGUI(Rect _position, GUIContent _label)
        {
            EditorGUILayout.LabelField(_label);
            float preWidth = 100;
            var target = Target as PropertyInfo;
            GUILayoutExtension.HorizontalGroup(() =>
            {
                EditorGUILayout.LabelField("Curr:", EditorStylesExtension.MiddleLabelStyle, GUILayout.Width(preWidth));
                EditorGUILayout.LabelField("Max:", EditorStylesExtension.MiddleLabelStyle, GUILayout.Width(preWidth));
                EditorGUILayout.LabelField("Min:", EditorStylesExtension.MiddleLabelStyle, GUILayout.Width(preWidth));
            }, EditorStylesExtension.NullStyle, GUILayout.Width(_position.width));

            GUILayoutExtension.HorizontalGroup(() =>
            {
                int currValue = EditorGUILayout.IntField(target.Curr, GUILayout.Width(preWidth));
                if (currValue != target.Curr)
                    target.Curr = currValue;

                int maxValue = EditorGUILayout.IntField(target.Max, GUILayout.Width(preWidth));
                if (maxValue != target.Max)
                    target.SetMax(maxValue);

                int minValue = EditorGUILayout.IntField(target.Min, GUILayout.Width(preWidth));
                if (minValue != target.Min)
                    target.SetMin(minValue);
            }, GUILayout.Width(_position.width));

            return Target;
        }

        public override float GetHeight()
        {
            return 15;
        }
    }
}