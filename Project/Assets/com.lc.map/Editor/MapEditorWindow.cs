using System;
using LCJson;
using LCToolkit;
using System.Collections.Generic;
using Demo;
using UnityEditor;
using UnityEngine;

namespace LCMap
{
    public class MapEditorWindow : EditorWindow
    {
        private static GUIHelper.ContextDataCache ContextDataCache = new GUIHelper.ContextDataCache();

        private static Dictionary<string, List<ActorCnf>> ActorDict = new Dictionary<string, List<ActorCnf>>();

        public static Action<ED_MapCom> OnSaveMapCallBack;
        
        public static Action<ED_MapCom,Dictionary<string, MapRoadTileInfo>> OnExportMapCallBack;

        [MenuItem("地图/编辑", true)]
        public static bool CheckHasSetting()
        {
            return MapSetting.Setting != null;
        }

        [MenuItem("地图/编辑")]
        public static void OpenMapEditorWindow()
        {
            MapEditorWindow prefabWin = GetWindow<MapEditorWindow>(false, "地图编辑", true);
            prefabWin.minSize = new Vector2(600, 250);
            prefabWin.Show();
        }

        private List<ED_MapCom> maps = new List<ED_MapCom>();

        private ED_MapCom currSelMap = null;

        private void OnEnable()
        {
            Refresh();
        }

        private void Refresh()
        {
            ActorDict = MapSetting.GetActorGroups();
            ActorFoldoutDict.Clear();
            foreach (var item in ActorDict)
            {
                ActorFoldoutDict.Add(item.Key,false);
            }
            maps = MapSetting.GetAllMaps();
        }

        private float BtnWidth = 100;
        private float BtnHeight = 50;
        private Dictionary<string, bool> ActorFoldoutDict = new Dictionary<string, bool>();
        private Vector2 scrollPos = Vector2.zero;
        private void OnGUI()
        {
            if (!ContextDataCache.TryGetContextData<GUIStyle>("BigLabel", out var bigLabel))
            {
                bigLabel.value = new GUIStyle(GUI.skin.label);
                bigLabel.value.fontSize = 18;
                bigLabel.value.fontStyle = FontStyle.Bold;
                bigLabel.value.alignment = TextAnchor.MiddleLeft;
                bigLabel.value.stretchWidth = true;
            }

            //工具栏
            GUILayoutExtension.HorizontalGroup(() =>
            {
                string selMap = currSelMap == null ? "Null" : currSelMap.mapId.ToString();
                GUILayout.Label($"当前地图:{selMap}", bigLabel.value);

                if (GUILayout.Button("刷新", GUILayout.Width(BtnWidth), GUILayout.Height(BtnHeight)))
                {
                    Refresh();
                }

                if (GUILayout.Button("加载地图", GUILayout.Width(BtnWidth), GUILayout.Height(BtnHeight)))
                {
                    List<string> mapNames = new List<string>();
                    for (int i = 0; i < maps.Count; i++)
                    {
                        mapNames.Add(maps[i].mapId.ToString());
                    }
                    MiscHelper.Menu(mapNames, (string selMap) =>
                    {
                        ED_MapCom mapAsset = GetMap(int.Parse(selMap));
                        GameObject mapGo = (GameObject)PrefabUtility.InstantiatePrefab(mapAsset.gameObject);
                        OnChangeMap(mapGo.GetComponent<ED_MapCom>());
                    });
                }

                if (currSelMap != null)
                {
                    if (GUILayout.Button("保存地图", GUILayout.Width(BtnWidth), GUILayout.Height(BtnHeight)))
                    {
                        SaveMap(currSelMap);
                        Refresh();
                    }
                }

                if (GUILayout.Button("新建地图", GUILayout.Width(BtnWidth), GUILayout.Height(BtnHeight)))
                {
                    MiscHelper.Input("输入地图Id", (string x) =>
                    {
                        if (GetMap(int.Parse(x)) != null)
                        {
                            Debug.LogError($"地图Id重复>>>>{x}");
                            return;
                        }
                        SaveMap(currSelMap);
                        ED_MapCom mapCom = MapEditorDef.CreateMapGo(x);
                        mapCom.SetUid(GetMapStartUid());
                        OnChangeMap(mapCom);
                    });
                }

                if (GUILayout.Button("导出所有配置", GUILayout.Width(BtnWidth), GUILayout.Height(BtnHeight)))
                {
                    MapRoadTileConfig roadTileConfig = AssetDatabase.LoadAssetAtPath<MapRoadTileConfig>("Assets/Demo/Editor/MapRoadTile/地图道路瓦片配置.asset");
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
                    
                    ED_MapCom.ActorCnfs = MapSetting.GetActorCnfs();
                    List<ED_MapCom> maps = MapSetting.GetAllMaps();
                    for (int i = 0; i < maps.Count; i++)
                    {
                        GameObject mapGo = GameObject.Instantiate(maps[i].gameObject);
                        ED_MapCom eD_MapCom = mapGo.GetComponent<ED_MapCom>();
                        MapInfo model = eD_MapCom.ExportData();

                        string filePath = MapSetting.GetMapModelSavePath(model.mapId.ToString());
                        IOHelper.WriteText(JsonMapper.ToJson(model), filePath);
                        
                        OnExportMapCallBack?.Invoke(eD_MapCom,mapRoadDict);
                        
                        DestroyImmediate(eD_MapCom.gameObject);
                        Debug.Log($"地图配置生成成功>>>>{filePath}");
                    }
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }

            },GUILayout.Height(50));

            GUILayoutExtension.HorizontalGroup(() =>
            {
                //演员分组列表
                GUILayoutExtension.VerticalGroup(() =>
                {
                    GUILayout.Label("演员列表", bigLabel.value);
                    GUILayoutExtension.ScrollView(ref scrollPos, () =>
                    {
                        foreach (var item in ActorDict)
                        {
                            string groupName = item.Key;
                            ActorFoldoutDict[groupName] = EditorGUILayout.Foldout(ActorFoldoutDict[groupName], groupName);
                            if (ActorFoldoutDict[groupName])
                            {
                                List<ActorCnf> actors = item.Value;
                                for (int i = 0; i < actors.Count; i++)
                                {
                                    if (GUILayout.Button($"{actors[i].name}-{actors[i].id}", GUILayout.Width(100), GUILayout.Height(BtnHeight)))
                                    {
                                        ED_ActorCom actorCom = currSelMap.CreateActor(actors[i]);
                                        Selection.activeObject = actorCom;
                                    }
                                }
                            }
                        }
                    });
                },GUILayout.Width(200));
            });
        }

        private void OnChangeMap(ED_MapCom newMap)
        {
            if (currSelMap != null)
            {
                SaveMap(currSelMap);
            }
            currSelMap = newMap;
            Selection.activeObject = currSelMap;
        }

        private void SaveMap(ED_MapCom map)
        {
            if (map == null)
                return;
            string mapPath = MapSetting.Setting.MapSearchPath + "/" + map.name + ".prefab";

            OnSaveMapCallBack?.Invoke(map);
            
            if (!PrefabUtility.IsPartOfPrefabInstance(map.gameObject))
            {
                PrefabUtility.SaveAsPrefabAsset(map.gameObject, mapPath);
            }
            else
            {
                PrefabUtility.ApplyPrefabInstance(map.gameObject, InteractionMode.AutomatedAction);
            }
            
            GameObject.DestroyImmediate(map.gameObject);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private ED_MapCom GetMap(int mapId)
        {
            for (int i = 0; i < maps.Count; i++)
            {
                if (maps[i].mapId == mapId)
                {
                    return maps[i];
                }
            }
            return null;
        }

        private int GetMapStartUid()
        {
            Refresh();
            int maxMapUid = MapEditorDef.MapUidCnt;
            for (int i = 0; i < maps.Count; i++)
            {
                if (maps[i].endUid > maxMapUid)
                {
                    maxMapUid = maps[i].endUid;
                }
            }
            return maxMapUid + 1;
        }
    }
}