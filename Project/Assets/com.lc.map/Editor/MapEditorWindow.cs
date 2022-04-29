using LCJson;
using LCToolkit;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace LCMap
{
    public class MapEditorWindow : EditorWindow
    {
        private static GUIHelper.ContextDataCache ContextDataCache = new GUIHelper.ContextDataCache();

        private static List<ActorCnf> ActorCnfs = new List<ActorCnf>();

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
            ActorCnfs = MapSetting.GetActorCnfs();
            maps = MapSetting.GetAllMaps();
        }

        private float BtnWidth = 100;
        private float BtnHeight = 50;
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
                    ED_MapCom.ActorCnfs = MapEditorWindow.ActorCnfs;
                    List<ED_MapCom> maps = MapSetting.GetAllMaps();
                    for (int i = 0; i < maps.Count; i++)
                    {
                        GameObject mapGo = GameObject.Instantiate(maps[i].gameObject);
                        ED_MapCom eD_MapCom = mapGo.GetComponent<ED_MapCom>();
                        MapModel model = eD_MapCom.ExportData();

                        string filePath = MapSetting.GetMapModelSavePath(model.mapId.ToString());
                        IOHelper.WriteText(JsonMapper.ToJson(model), filePath);

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
                    for (int i = 0; i < ActorCnfs.Count; i++)
                    {
                        if (GUILayout.Button($"{ActorCnfs[i].name}-{ActorCnfs[i].id}", GUILayout.Width(100), GUILayout.Height(BtnHeight)))
                        {
                            ED_ActorCom actorCom = currSelMap.CreateActor(ActorCnfs[i]);
                            Selection.activeObject = actorCom;
                        }
                    }
                },GUILayout.Width(50));
                
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
            PrefabUtility.SaveAsPrefabAsset(map.gameObject, mapPath);
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