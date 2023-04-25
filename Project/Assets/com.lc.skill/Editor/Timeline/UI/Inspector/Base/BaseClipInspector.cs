using System.Reflection;
using LCToolkit;
using LCToolkit.Core;
using UnityEditor;
using UnityEngine;

namespace LCSkill.Timeline.Inspector
{
    [CustomInspectorDrawer(typeof(BaseClip))]
    public class BaseClipInspector : ObjectInspectorDrawer
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

            BaseClip clipModel = Target as BaseClip;
            GUILayoutExtension.VerticalGroup(() =>
            {
                GUILayout.Label("Clip:" + clipModel.name, bigLabel.value);
            });

            int tStart = EditorGUILayout.IntField("Start", clipModel.startFrame);
            if (tStart != clipModel.startFrame)
            {
                clipModel.startFrame = tStart;
            }

            int tEnd = EditorGUILayout.IntField("End", clipModel.endFrame);
            if (tEnd != clipModel.endFrame)
            {
                clipModel.endFrame = tEnd;
            }

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.IntField("Duration", clipModel.DurationFrame);
            EditorGUI.EndDisabledGroup();

            foreach (var field in Fields)
            {
                if (field.Name == "startFrame" || field.Name == "endFrame" || field.Name == "DurationFrame")
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