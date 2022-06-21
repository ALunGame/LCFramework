using LCToolkit;
using UnityEditor;
using UnityEngine;

namespace Demo
{
    [CustomEditor(typeof(MapRoadTileConfig), true)]
    public class MapRoadTileConfigInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            MapRoadTileConfig roadTileConfig = (MapRoadTileConfig)target;

            GUILayoutExtension.HorizontalGroup(() =>
            {
                if (GUILayout.Button("添加"))
                {
                    MapRoadTileInfo newRoadInfo = new MapRoadTileInfo();
                    newRoadInfo.roadPos = new Vector2(0, 0.5f);
                    roadTileConfig.tiles.Add(newRoadInfo);
                }
                if (GUILayout.Button("保存"))
                {
                    EditorUtility.SetDirty(roadTileConfig);
                    AssetDatabase.Refresh();
                    AssetDatabase.SaveAssets();
                }
            });

            GUILayoutExtension.VerticalGroup(() =>
            {
                for (int i = 0; i < roadTileConfig.tiles.Count; i++)
                {
                    DrawMapRoadInfo(roadTileConfig.tiles[i]);
                }
            });
        }

        private void DrawMapRoadInfo(MapRoadTileInfo roadInfo)
        {
            GUILayoutExtension.VerticalGroup(() =>
            {
                GUILayout.Label(roadInfo.sprite == null ? "" : roadInfo.sprite.name);
                roadInfo.sprite = (Sprite)EditorGUILayout.ObjectField("道路检测图:", roadInfo.sprite,typeof(Sprite),false);
                roadInfo.roadPos = EditorGUILayout.Vector2Field("道路位置:", roadInfo.roadPos);
                roadInfo.animName = EditorGUILayout.TextField("移动动画", roadInfo.animName);

                if (GUILayout.Button("删除"))
                {
                    MapRoadTileConfig roadTileConfig = (MapRoadTileConfig)target;
                    roadTileConfig.tiles.Remove(roadInfo);
                }
            });
        }
    }
}
