using System.Reflection;
using LCToolkit;
using LCToolkit.Core;
using UnityEditor;
using UnityEngine;

namespace LCSkill.Timeline.Inspector
{
    [CustomInspectorDrawer(typeof(BaseTimeline))]
    public class BaseTimelineInspector : ObjectInspectorDrawer
    {
        static GUIHelper.ContextDataCache ContextDataCache = new GUIHelper.ContextDataCache();
        public override void OnEnable()
        {
            base.OnEnable();
        }

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

            BaseTimeline timelineModel = Target as BaseTimeline;
            GUILayoutExtension.VerticalGroup(() =>
            {
                GUILayout.Label("Timeline:" + timelineModel.name, bigLabel.value);
            });
            
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.IntField("TotalFrame", timelineModel.totalFrame);
            EditorGUI.EndDisabledGroup();
            
            foreach (var field in Fields)
            {
                if (field.Name == "name" || field.Name == "totalFrame" || field.Name == "previewGo")
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

        public override void OnDisable()
        {
            base.OnDisable();
        }
    }
}