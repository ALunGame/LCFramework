#if UNITY_EDITOR
using LCToolkit;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LCMap
{
    /// <summary>
    /// 地图区域组件
    /// 1，一个地图被划分为任意多个区域
    /// </summary>
    [ExecuteAlways]
    public class ED_MapAreaCom : ED_MapDataCom
    {
        [EDReadOnly]
        [SerializeField]
        private ED_MapCom mapCom;

        [EDReadOnly]
        [SerializeField]
        [Header("演员根节点")]
        public Transform ActorRoot;

        [EDReadOnly]
        [SerializeField]
        [Header("触发区域根节点")]
        public Transform TriggerRoot;

        [SerializeField]
        [Header("区域环境")]
        public GameObject AreaEnv;
        private GameObject tmpAreaEnv;

        private Rect areaRect;

        private void Awake()
        {
            ActorRoot = transform.Find("Actors");
            TriggerRoot = transform.Find("Triggers");
            if (AreaEnv != null)
            {
                tmpAreaEnv = (GameObject)PrefabUtility.InstantiatePrefab(AreaEnv);
            }
        }

        private void Update()
        {
            Vector2Int areaSize = mapCom.AreaSize;
            Vector3 centerPos = transform.position - new Vector3(areaSize.x, areaSize.y) / 2;
            areaRect = new Rect(centerPos, areaSize);

            ED_ActorCom[] actors = GetActors();
            if (actors!=null || actors.Length>0)
            {
                for (int i = 0; i < actors.Length; i++)
                {
                    ED_ActorCom actorCom = actors[i];
                    actorCom.transform.position = ClampPos(actorCom.transform.position);
                }
            }

            ED_MapTriggerCom[] triggers = GetTriggers();
            if (triggers != null || triggers.Length > 0)
            {
                for (int i = 0; i < triggers.Length; i++)
                {
                    ED_MapTriggerCom triggerCom = triggers[i];
                    triggerCom.transform.position = ClampPos(triggerCom.transform.position);
                }
            }
        }

        private void OnDestroy()
        {
            if (tmpAreaEnv != null)
            {
                GameObject.DestroyImmediate(tmpAreaEnv);
            }
        }

        private Vector3 ClampPos(Vector3 pos)
        {
            if (!areaRect.Contains(pos))
            {
                return areaRect.center;
            }
            return pos;
        }

        public ED_ActorCom[] GetActors()
        {
            return ActorRoot.GetComponentsInChildren<ED_ActorCom>(true);
        }

        public ED_MapTriggerCom[] GetTriggers()
        {
            return TriggerRoot.GetComponentsInChildren<ED_MapTriggerCom>(true);
        }

        public void AddActor(ED_ActorCom actorCom)
        {
            MapEditorHelper.SetParent(actorCom.gameObject, ActorRoot.gameObject);
        }

        public void AddTrigger(ED_MapTriggerCom triggerCom)
        {
            MapEditorHelper.SetParent(triggerCom.gameObject, TriggerRoot.gameObject);
        }

        private void OnDrawGizmos()
        {
            GizmosHelper.DrawRect(areaRect, Color.red);
        }

        public override object ExportData()
        {
            AreaModel areaModel = new AreaModel();
            areaModel.areaPrefab = AreaEnv.name;
            //演员
            ED_ActorCom[] actorComs = ActorRoot.GetComponentsInChildren<ED_ActorCom>(true);
            if (actorComs != null)
            {
                for (int i = 0; i < actorComs.Length; i++)
                {
                    ED_ActorCom tActorCom = actorComs[i];
                    areaModel.actors.Add((ActorModel)tActorCom.ExportData());
                }
            }

            //地图默认演员
            List<ActorModel> mapActors = CollectMapDefaultActors(tmpAreaEnv);
            if (mapActors != null && mapActors.Count > 0)
            {
                foreach (ActorModel actorData in mapActors)
                {
                    areaModel.actors.Add(actorData);
                }
            }

            //触发区域
            ED_MapTriggerCom[] triggerComs = TriggerRoot.GetComponentsInChildren<ED_MapTriggerCom>(true);
            if (triggerComs != null)
            {
                for (int i = 0; i < triggerComs.Length; i++)
                {
                    ED_MapTriggerCom tTriggerCom = triggerComs[i];
                    areaModel.triggers.Add(i + 1, (MapTriggerModel)tTriggerCom.ExportData());
                }
            }

            return areaModel;
        }

        private int defaultActorUid = -100;
        private List<ActorModel> CollectMapDefaultActors(GameObject mapGo)
        {
            defaultActorUid = -100;
            if (mapGo == null)
            {
                Debug.LogError("地图导出失败，没有地图预制体" + name);
                return null;
            }
            List<ActorModel> actors = new List<ActorModel>();

            Transform actorRoot = mapGo.transform.Find("DefaultActor");
            if (actorRoot != null)
            {
                for (int i = 0; i < actorRoot.childCount; i++)
                {
                    Transform actor = actorRoot.GetChild(i).transform;
                    ActorAsset actorModel = MapEditorDef.GetActorAsset(actor.name);
                    if (actorModel != null)
                    {
                        ActorModel actorData = new ActorModel();
                        actorData.uid = defaultActorUid--;
                        actorData.id = actorModel.actorId;
                        actorData.pos = ED_ActorCom.HandlePos(transform.position);
                        actorData.roate = ED_ActorCom.HandlePos(transform.localEulerAngles);
                        actorData.scale = ED_ActorCom.HandlePos(transform.localScale);
                        actorData.isActive = gameObject.activeSelf;

                        actors.Add(actorData);
                    }
                }
            }
            return actors;
        }
    }
} 
#endif