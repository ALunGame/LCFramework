#if UNITY_EDITOR
using LCToolkit;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LCMap
{
    /// <summary>
    /// 地图组件
    /// </summary>
    [ExecuteAlways]
    public class ED_MapCom : MonoBehaviour
    {
        #region Static
        public static List<ActorCnf> ActorCnfs = new List<ActorCnf>();

        public static ActorCnf GetActorCnf(string name)
        {
            for (int i = 0; i < ActorCnfs.Count; i++)
            {
                if (ActorCnfs[i].name == name)
                {
                    return ActorCnfs[i];
                }
            }
            return null;
        }

        #endregion

        [ReadOnly]
        [SerializeField]
        [Header("地图Id")]
        public int mapId;

        [ReadOnly]
        [SerializeField]
        [Header("起始Uid")]
        public int startUid = 0;

        [ReadOnly]
        [SerializeField]
        [Header("结束Uid")]
        public int endUid;

        [ReadOnly]
        [SerializeField]
        [Header("区域根节点")]
        public Transform AreaRoot;

        [ReadOnly]
        [SerializeField]
        [Header("区域范围")]
        public Vector2Int AreaSize = new Vector2Int(100, 100);

        private void Awake()
        {
            AreaRoot = transform.Find("Areas");
        }

        public void SetUid(int startUid)
        {
            this.startUid = startUid;
            this.endUid   = MapEditorDef.MapUidCnt + startUid - 1;
        }

        private void OnDestroy()
        {
        }

        public int CalcActorUid()
        {
            int uid = startUid + 1;
            if (uid >= endUid)
            {
                Debug.LogError($"Uid生成失败，超过最大配额{uid}");
                return 0;
            }
            startUid = uid;
            return uid;
        }

        public ED_MapAreaCom GetArea(int index = -1)
        {
            for (int i = 0; i < AreaRoot.transform.childCount; i++)
            {
                ED_MapAreaCom areaCom = AreaRoot.transform.GetChild(i).GetComponent<ED_MapAreaCom>();
                if (areaCom != null)
                {
                    if (index == i)
                    {
                        return areaCom;
                    }
                    if (areaCom.gameObject.activeInHierarchy)
                    {
                        return areaCom;
                    }
                }
            }
            return null;
        }

        public List<ED_ActorCom> GetActors()
        {
            List<ED_ActorCom> actors = new List<ED_ActorCom>();
            for (int i = 0; i < AreaRoot.transform.childCount; i++)
            {
                ED_MapAreaCom areaCom = AreaRoot.transform.GetChild(i).GetComponent<ED_MapAreaCom>();
                if (areaCom != null)
                {
                    actors.AddRange(areaCom.GetActors());
                }
            }
            return actors;
        }

        public ED_ActorCom CreateActor(ActorCnf actorData)
        {
            ED_MapAreaCom areaCom = GetArea();
            if(areaCom == null)
            {
                Debug.LogError("创建地图失败，没有区域！！！！");
                return null;
            }
            if (startUid > endUid)
            {
                Debug.LogError("此地图Uid已用完！！！！");
                return null;
            }

            ED_ActorCom actorCom = MapEditorDef.CreateActorGo();
            actorCom.Init(CalcActorUid(), actorData.id, actorData.name, actorData.isPlayer, this);

            //预制体
            if (actorData.prefab != null)
            {
                GameObject actorPrefab = (GameObject)PrefabUtility.InstantiatePrefab(actorData.prefab.GetObj());
                MapEditorHelper.SetParent(actorPrefab.gameObject, actorCom.gameObject);
            }

            areaCom.AddActor(actorCom);

            return actorCom;
        }

        public ED_MapTriggerCom CreateTrigger()
        {
            ED_MapAreaCom areaCom = GetArea();
            if (areaCom == null)
            {
                Debug.LogError("创建地图失败，没有区域！！！！");
                return null;
            }
            Transform obsTrans = transform.Find("Prefab/TriggerArea");
            GameObject newGo = Instantiate(obsTrans.gameObject);
            ED_MapTriggerCom triggerCom = newGo.GetComponent<ED_MapTriggerCom>();
            areaCom.AddTrigger(triggerCom);
            return triggerCom;
        }

        #region 数据导出

        public MapModel ExportData()
        {
            MapModel mapData = new MapModel();
            mapData.mapId = mapId;

            for (int i = 0; i < AreaRoot.transform.childCount; i++)
            {
                ED_MapAreaCom areaCom = AreaRoot.transform.GetChild(i).GetComponent<ED_MapAreaCom>();
                if (areaCom != null)
                {
                    AreaModel areaModel = (AreaModel)areaCom.ExportData();
                    areaModel.areaId = i;
                    mapData.areas.Add(areaModel);
                }
            }

            //主角
            mapData.mainActor = null;
            for (int i = 0; i < mapData.areas.Count; i++)
            {
                for (int j = 0; j < mapData.areas[i].actors.Count; j++)
                {
                    if (mapData.areas[i].actors[j].isMainActor)
                    {
                        if (mapData.mainActor!=null)
                        {
                            Debug.LogWarning($"具有多个主角>>>>>{mapData.areas[i].areaId}");
                        }
                        mapData.mainActor = mapData.areas[i].actors[j];
                        mapData.areas[i].actors.RemoveAt(j);
                    }
                }
            }

            return mapData;
        }

        #endregion
    }
} 
#endif
