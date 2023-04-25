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
        [ReadOnly]
        [SerializeField]
        private ED_MapCom mapCom;

        [ReadOnly]
        [SerializeField]
        [Header("演员根节点")]
        public Transform ActorRoot;

        [ReadOnly]
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
            UpdateRect();
            if (AreaEnv != null)
            {
                tmpAreaEnv = (GameObject)PrefabUtility.InstantiatePrefab(AreaEnv);
            }
        }

        private void Update()
        {
            UpdateRect();
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
            GizmosHelper.DrawRect(areaRect, Color.yellow);
        }

        private void UpdateRect()
        {
            Vector2Int areaSize = mapCom.AreaSize;
            Vector3 centerPos = transform.position - new Vector3(areaSize.x, areaSize.y) / 2;
            areaRect = new Rect(centerPos, areaSize);
        }

        public override object ExportData()
        {
            UpdateRect();

            AreaInfo areaModel = new AreaInfo();
            areaModel.pos = transform.localPosition;
            areaModel.rect = areaRect;
            areaModel.areaPrefab = AreaEnv.name;

            //演员
            ED_ActorCom[] actorComs = ActorRoot.GetComponentsInChildren<ED_ActorCom>(true);
            if (actorComs != null)
            {
                for (int i = 0; i < actorComs.Length; i++)
                {
                    ED_ActorCom tActorCom = actorComs[i];
                    areaModel.actors.Add((ActorInfo)tActorCom.ExportData());
                }
            }

            //地图默认演员
            List<ActorInfo> mapActors = CollectMapDefaultActors(tmpAreaEnv);
            if (mapActors != null && mapActors.Count > 0)
            {
                foreach (ActorInfo actorData in mapActors)
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
                    areaModel.triggers.Add(i + 1, (MapTriggerInfo)tTriggerCom.ExportData());
                }
            }

            return areaModel;
        }

        private int defaultActorUid = -100;
        private List<ActorInfo> CollectMapDefaultActors(GameObject mapGo)
        {
            defaultActorUid = -100;
            if (mapGo == null)
            {
                Debug.LogError("地图导出失败，没有地图预制体" + name);
                return null;
            }
            List<ActorInfo> actors = new List<ActorInfo>();

            Transform actorRoot = mapGo.transform.Find("DefaultActor");
            if (actorRoot != null)
            {
                for (int i = 0; i < actorRoot.childCount; i++)
                {
                    Transform actor = actorRoot.GetChild(i).transform;
                    ActorCnf actorCnf = ED_MapCom.GetActorCnf(actor.name);
                    if (actorCnf != null)
                    {
                        ActorInfo actorData = new ActorInfo();
                        actorData.uid   = $"actor_{defaultActorUid--}";
                        actorData.id    = actorCnf.id;
                        actorData.pos   = ED_ActorCom.HandlePos(transform.position);
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