using LCToolkit;
using LCToolkit.Core;
using UnityEngine;


namespace LCTimeline.Inspector
{
    [CustomInspectorDrawer(typeof(TrackModel))]
    public class BaseTimelineTrackInspector : ObjectInspectorDrawer
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

            TrackModel trackModel = Target as TrackModel;
            GUILayoutExtension.VerticalGroup(() =>
            {
                GUILayout.Label("Track:"+trackModel.TitleName, bigLabel.value);
            });

            base.OnInspectorGUI();
        }
    }
}
