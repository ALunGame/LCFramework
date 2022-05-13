using LCNode.Model;
using LCNode.Model.Internal;
using LCNode.View;
using UnityEditor;
using UnityEngine;
using LCToolkit;

namespace LCTimeline.Inspector
{
    [CustomEditor(typeof(InternalTimelineGraphAsset), true)]
    public class BaseTimelineGraphAssetInspector : Editor
    {
        static GUIHelper.ContextDataCache ContextDataCache = new GUIHelper.ContextDataCache();

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!ContextDataCache.TryGetContextData<GUIStyle>("BigLabel", out var bigLabel))
            {
                bigLabel.value = new GUIStyle(GUI.skin.label);
                bigLabel.value.fontSize = 18;
                bigLabel.value.fontStyle = FontStyle.Bold;
                bigLabel.value.alignment = TextAnchor.MiddleLeft;
                bigLabel.value.stretchWidth = true;
            }

            ITimelineGraphAsset graphAsset = target as ITimelineGraphAsset;

            if (GUILayout.Button("Open", GUILayout.Height(30)))
            {
                TimelineWindow.Open(graphAsset);
            }
        }
    }
}
