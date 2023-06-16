using System;
using System.Collections.Generic;
using AStar;
using Demo.AStar;
using Demo.AStar.Com;
using LCConfig.Excel;
using LCConfig.Excel.GenCode;
using LCMap;
using LCToolkit;
using MemoryPack;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Demo.Map
{
    public static class MapEditorFunc
    {
        private const string roadRuleCnfPath = "Assets/Demo/Editor/MapRoadTile/地图道路瓦片配置.asset";
        private const string astarTxPath = "Assets/Demo/Asset/Maps/Common/Tile/Sprite/tile_astar.png";

        [InitializeOnLoadMethod]
        private static void OnSaveMap()
        {
            MapEditorWindow.OnSaveMapCallBack += HandleSaveMap;
        }

        private static void HandleSaveMap(ED_MapCom pMapCom)
        {
            ED_MapAreaCom[] areaComs = pMapCom.AreaRoot.GetComponentsInChildren<ED_MapAreaCom>();

            for (int i = 0; i < areaComs.Length; i++)
            {
                if (areaComs[i].AreaEnv != null)
                {
                    PathGridCom pathGridCom = areaComs[i].AreaEnv.GetComponentInChildren<PathGridCom>();
                    if (pathGridCom != null)
                    {
                        RefreshAStarTileMap(pathGridCom);
                        pathGridCom.gridRect = pathGridCom.CalcGridRect(pathGridCom);
                    }
                }
            }
        }
        
        [InitializeOnLoadMethod]
        private static void OnExportMap()
        {
            MapEditorWindow.OnExportMapCallBack += HandleExportMap;
        }

        private static void HandleExportMap(ED_MapCom pMapCom, Dictionary<string, MapRoadTileInfo> pMapRoadDict)
        {
            ED_MapAreaCom[] areaComs = pMapCom.AreaRoot.GetComponentsInChildren<ED_MapAreaCom>();
            MapRoadCnf mapRoadCnf = new MapRoadCnf();
            mapRoadCnf.mapId = pMapCom.mapId;
            
            for (int i = 0; i < areaComs.Length; i++)
            {
                if (areaComs[i].AreaEnv != null)
                {
                    PathGridCom pathGridCom = areaComs[i].AreaEnv.GetComponentInChildren<PathGridCom>();
                    if (pathGridCom != null)
                    {
                        MapAreaRoadCnf mapAreaRoadCnf = ExportMapRoadCnf(pathGridCom, pMapRoadDict);
                        mapAreaRoadCnf.areaId = i;
                        mapRoadCnf.areaRoads.Add(mapAreaRoadCnf);
                    }
                }
            }
            
            string savePath = $"{ExcelReadSetting.Setting.GenJsonRootPath}/{GetFileName(pMapCom.mapId)}";
            var data = MemoryPackSerializer.Serialize(mapRoadCnf.GetType(), mapRoadCnf);
            IOHelper.WriteBytes(data,savePath);
        }

        private static string GetFileName(int mapId)
        {
            return $"{ExcelGenCode.Tb}MapRoad_{mapId}{ExcelReadSetting.Setting.GenJsonExName}";
        }

        private static void RefreshAStarTileMap(PathGridCom pGridCom)
        {
            MapRoadTileConfig roadTileConfig = AssetDatabase.LoadAssetAtPath<MapRoadTileConfig>(roadRuleCnfPath);
            Sprite txSp = AssetDatabase.LoadAssetAtPath<Sprite>(astarTxPath);
            
            
            Dictionary<string, MapRoadTileInfo> mapRoadDict = new Dictionary<string, MapRoadTileInfo>();
            foreach (MapRoadTileInfo tile in roadTileConfig.tiles)
            {
                if (mapRoadDict.ContainsKey(tile.sprite.name))
                {
                    Debug.LogError("重复的地图道路" + tile.sprite.name);
                }
                else
                {
                    mapRoadDict.Add(tile.sprite.name, tile);
                }
            }

            Tilemap aStarTileMap = pGridCom.gridTilemap;
            aStarTileMap.ClearAllTiles();
            
            Tilemap tilemap = pGridCom.envTilemap;
            for (int i = tilemap.cellBounds.xMin; i < tilemap.cellBounds.xMax; i++)
            {
                for (int j = tilemap.cellBounds.yMin; j < tilemap.cellBounds.yMax; j++)
                {
                    Vector3Int tilePos = new Vector3Int(i, j);
                    Sprite sprite = tilemap.GetSprite(tilePos);
                    if (sprite != null && mapRoadDict.ContainsKey(sprite.name))
                    {
                        MapRoadTileInfo info = mapRoadDict[sprite.name];
                        Vector3Int checkTilePos = new Vector3Int(i + info.roadPos.x, j + info.roadPos.y, 0);
                            
                        if (tilemap.GetTile(checkTilePos) == null)
                        {
                            Tile tile = new Tile();
                            tile.sprite = txSp;
                            tile.color = info.roadType == MapRoadType.Obstruct ? Color.black : Color.green;
                            aStarTileMap.SetTile(checkTilePos,tile);
                        }
                    }
                }
            }
        }

        private static MapAreaRoadCnf ExportMapRoadCnf(PathGridCom pGridCom, Dictionary<string, MapRoadTileInfo> pMapRoadDict)
        {
            Tilemap tilemap = pGridCom.gridTilemap;

            MapAreaRoadCnf mapAreaRoadCnf = new MapAreaRoadCnf();
            for (int i = tilemap.cellBounds.xMin; i < tilemap.cellBounds.xMax; i++)
            {
                for (int j = tilemap.cellBounds.yMin; j < tilemap.cellBounds.yMax; j++)
                {
                    Vector3Int tilePos = new Vector3Int(i, j);
                    Sprite sprite = tilemap.GetSprite(tilePos);
                    if (sprite != null && pMapRoadDict.ContainsKey(sprite.name))
                    {
                        MapRoadTileInfo info = pMapRoadDict[sprite.name];
                        Vector3Int checkTilePos = new Vector3Int(i + info.roadPos.x, j + info.roadPos.y, 0);
                            
                        if (tilemap.GetSprite(checkTilePos) == null)
                        {
                            Vector3 roadWorldPos = tilemap.CellToWorld(checkTilePos);
                            roadWorldPos += new Vector3(tilemap.layoutGrid.cellSize.x * tilemap.tileAnchor.x,
                                tilemap.layoutGrid.cellSize.y * tilemap.tileAnchor.y - tilemap.layoutGrid.cellSize.y/2, 0);
                                
                            RoadCnf cnf       = new RoadCnf();
                            cnf.tilePos          = new Vector2Int(checkTilePos.x, checkTilePos.y);
                            cnf.roadPos          = roadWorldPos;
                            cnf.roadType         = info.roadType;
                            mapAreaRoadCnf.roads.Add(cnf);
                        }
                    }
                }
            }

            return mapAreaRoadCnf;
        }
    }
}