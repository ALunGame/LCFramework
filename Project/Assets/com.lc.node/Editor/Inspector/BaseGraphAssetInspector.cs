using LCNode.Model;
using LCNode.Model.Internal;
using LCNode.View;
using UnityEditor;
using UnityEngine;
using LCToolkit;
using LCToolkit;

namespace LCNode.Inspector
{
    [CustomEditor(typeof(InternalBaseGraphAsset), true)]
    public class BaseGraphAssetInspector : Editor
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

            IGraphAsset graphAsset = target as IGraphAsset;

            if (GUILayout.Button("Open", GUILayout.Height(30)))
                BaseGraphWindow.Open(target as InternalBaseGraphAsset);
        }
    }
}
