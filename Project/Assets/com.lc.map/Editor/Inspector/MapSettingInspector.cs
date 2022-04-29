using LCToolkit;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace LCMap
{
    [CustomEditor(typeof(MapSetting), true)]
    public class MapSettingInspector : Editor
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

            if (GUILayout.Button("´´½¨Ä¿Â¼", GUILayout.Height(30)))
            {
                MapSetting mapSetting = target as MapSetting;
                Directory.CreateDirectory(mapSetting.MapSearchPath);
                Directory.CreateDirectory(mapSetting.MapExportSavePath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
    } 
}
