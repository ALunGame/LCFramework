using LCToolkit;
using LCToolkit.Core;
using UnityEngine;

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

            base.OnInspectorGUI();
        }
    }
}
