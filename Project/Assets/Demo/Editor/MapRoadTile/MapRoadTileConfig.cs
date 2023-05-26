using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using LCToolkit;
using UnityEngine.Tilemaps;
using LCJson;
using Demo.System;
using Map;

namespace Demo
{
    [Serializable]
    public class MapRoadTileInfo
    {
        public Sprite sprite;
        public Vector2Int roadPos;
        public MapRoadType roadType = MapRoadType.Normal;
    }

    [CreateAssetMenu(fileName = "地图道路瓦片配置", menuName = "游戏/地图道路瓦片配置", order = 1)]
    public class MapRoadTileConfig : ScriptableObject
    {
        public List<MapRoadTileInfo> tiles = new List<MapRoadTileInfo>();

        [MenuItem("CONTEXT/Tilemap/导出地图道路")]
        private static void CONTEXT_MeshFilter_right_btn()
        {
            // MapRoadTileConfig roadTileConfig = AssetDatabase.LoadAssetAtPath<MapRoadTileConfig>("Assets/Demo/Editor/MapRoadTile/地图道路瓦片配置.asset");
            // Dictionary<string, MapRoadTileInfo> mapRoadDict = new Dictionary<string, MapRoadTileInfo>();
            // foreach (MapRoadTileInfo tile in roadTileConfig.tiles)
            // {
            //     if (mapRoadDict.ContainsKey(tile.sprite.name))
            //     {
            //         Debug.LogError("重复的地图道路" + tile.sprite.name);
            //     }
            //     else
            //     {
            //         mapRoadDict.Add(tile.sprite.name, tile);
            //     }
            // }
            //
            //
            // MiscHelper.Input("输入地图Id", (x) =>
            // {
            //     GameObject selGo = Selection.activeGameObject;
            //     Tilemap tilemap = selGo.GetComponent<Tilemap>();
            //
            //     List<MapRoadCnf> roadCnfs = new List<MapRoadCnf>();
            //     for (int i = tilemap.cellBounds.xMin; i < tilemap.cellBounds.xMax; i++)
            //     {
            //         for (int j = tilemap.cellBounds.yMin; j < tilemap.cellBounds.yMax; j++)
            //         {
            //             Vector3Int tilePos = new Vector3Int(i, j);
            //             Sprite sprite = tilemap.GetSprite(tilePos);
            //             if (sprite != null && mapRoadDict.ContainsKey(sprite.name))
            //             {
            //                 MapRoadTileInfo info = mapRoadDict[sprite.name];
            //                 Vector3Int checkTilePos = new Vector3Int(i + info.roadPos.x, j + info.roadPos.y, 0);
            //                 
            //                 if (tilemap.GetSprite(checkTilePos) == null)
            //                 {
            //                     Vector3 roadWorldPos = tilemap.CellToWorld(checkTilePos);
            //                     roadWorldPos += new Vector3(tilemap.layoutGrid.cellSize.x * tilemap.tileAnchor.x,
            //                         tilemap.layoutGrid.cellSize.y * tilemap.tileAnchor.y - tilemap.layoutGrid.cellSize.y/2, 0);
            //                     
            //                     MapRoadCnf cnf       = new MapRoadCnf();
            //                     cnf.tilePos          = new Vector2Int(checkTilePos.x, checkTilePos.y);
            //                     cnf.roadPos          = roadWorldPos;
            //                     cnf.roadType         = info.roadType;
            //                     roadCnfs.Add(cnf);
            //                     
            //                     Debug.Log("Pos》》》》》" + tilemap.CellToWorld(checkTilePos));
            //                     Debug.Log("roadPos》》》》》" + cnf.roadPos);
            //                 }
            //             }
            //         }
            //     }
            //
            //     GameObject roadRoot = new GameObject($"Map_{x}_Road");
            //     for (int i = 0; i < roadCnfs.Count; i++)
            //     {
            //         MapRoadCnf cnf = roadCnfs[i];
            //         GameObject roadGo = new GameObject($"{cnf.roadPos}");
            //         roadGo.transform.SetParent(roadRoot.transform);
            //         roadGo.transform.position = cnf.roadPos;
            //         
            //         EditorSelectIcon.SetIcon(roadGo, EditorSelectIcon.Icon.CircleBlue);
            //     }
            //
            //     string jsonStr = JsonMapper.ToJson(roadCnfs);
            //     IOHelper.WriteText(jsonStr, WayPointMoveSystem.CnfSavePath);
            //     Debug.Log("》》》》》" + WayPointMoveSystem.CnfSavePath);
            //     AssetDatabase.SaveAssets();
            //     AssetDatabase.Refresh();
            // });
        }


    } 
}
