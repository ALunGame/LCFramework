using LCToolkit;
using LCToolkit.Core;
using UnityEngine;
using UnityEditor;
using System.Reflection;

namespace LCTimeline.Inspector
{
    [CustomInspectorDrawer(typeof(ClipModel))]
    public class BaseTimelineClipInspector : ObjectInspectorDrawer
    {
        static GUIHelper.ContextDataCache ContextDataCache = new GUIHelper.ContextDataCache();

        public override void OnInspectorGUI()
        {
            if (!ContextDataCache.TryGetContextData<GUIStyle>("BigLabel", out var bigLabel))
            {
                bigLabel.value = new GUIStyle(GUI.skin.label);
                bigLabel.value.fontSize = 18;
                bigLabel.value.fontStyle = FontStyle.Bold;
                bigLabel.value.alignment = TextAnchor.MiddleLeft;
                bigLabel.value.stretchWidth = true;
            }

            ClipModel clipModel = Target as ClipModel;
            GUILayoutExtension.VerticalGroup(() =>
            {
                GUILayout.Label("Clip:" + clipModel.TitleName, bigLabel.value);
            });

            float tStart = EditorGUILayout.FloatField("Start", clipModel.StartTime);
            if (tStart != clipModel.StartTime)
            {
                clipModel.SetStart((float)tStart);
            }

            float tEnd = EditorGUILayout.FloatField("End", clipModel.EndTime);
            if (tEnd != clipModel.EndTime)
            {
                clipModel.SetEnd((float)tEnd);
            }

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.FloatField("Duration", clipModel.DurationTime);
            EditorGUI.EndDisabledGroup();

            foreach (var field in Fields)
            {
                if (field.Name == "StartTime" || field.Name == "EndTime" || field.Name == "DurationTime")
                {
                    continue;
                }
                DrawFields(field);
            }
        }

        public virtual void DrawFields(FieldInfo fieldInfo)
        {
            GUILayoutExtension.DrawField(fieldInfo, Target);
        }
    }
}
