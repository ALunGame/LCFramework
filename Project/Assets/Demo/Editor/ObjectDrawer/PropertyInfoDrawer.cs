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
        public override void OnGUI(Rect _position, GUIContent _label)
        {
            base.OnGUI(_position, _label);

            Rect preRect = new Rect(0, _position.y, _position.width / 3, _position.height);

            var target  = Target as PropertyInfo;
            GUILayoutExtension.VerticalGroup(() =>
            {
                int maxValue = EditorGUILayout.IntField(GUIHelper.TextContent("Max:", $"{_label.text}最大值"), target.Max);
                if (maxValue != target.Max)
                    target.SetMax(maxValue);

                int minValue = EditorGUILayout.IntField(GUIHelper.TextContent("Min:", $"{_label.text}最小值"), target.Min);
                if (minValue != target.Min)
                    target.SetMin(minValue);

                int currValue = EditorGUILayout.IntField(GUIHelper.TextContent("Curr:", $"{_label.text}初始值"), target.Curr);
                if (currValue != target.Curr)
                    target.Curr = currValue;
            });
        }

        public override float GetHeight()
        {
            return 15*3;
        }
    }
}