using System.Reflection;
using LCToolkit;
using LCToolkit.Core;
using UnityEditor;
using UnityEngine;

namespace LCSkill.Timeline.Inspector
{
    [CustomInspectorDrawer(typeof(BaseTrack))]
    public class BaseTrackInspector : ObjectInspectorDrawer
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

            BaseTrack trackModel = Target as BaseTrack;
            GUILayoutExtension.VerticalGroup(() =>
            {
                GUILayout.Label("Track:" + trackModel.trackName, bigLabel.value);
            });
            
            foreach (var field in Fields)
            {
                if (field.Name == "trackName" || field.Name == "TrackName")
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