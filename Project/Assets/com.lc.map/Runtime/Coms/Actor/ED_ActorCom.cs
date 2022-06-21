#if UNITY_EDITOR
using LCToolkit;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LCMap
{
    /// <summary>
    /// 地图演员
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [ExecuteAlways]
    public class ED_ActorCom : ED_MapDataCom
    {
        [ReadOnly]
        [SerializeField]
        [Header("演员Uid")]
        public string UId;

        [ReadOnly]
        [SerializeField]
        [Header("演员Id")]
        public int Id;

        [ReadOnly]
        [SerializeField]
        [Header("演员名字")]
        public string ActorName = "";

        [ReadOnly]
        [SerializeField]
        [Header("演员类型")]
        public ActorType Type;

        [SerializeField]
        [Header("表现状态名")]
        public string stateName = "Default";

        [ReadOnly]
        [SerializeField]
        [Header("路径根节点")]
        public Transform PathRoot;

        [ReadOnly]
        [SerializeField]
        [Header("交互点")]
        public Transform InteractivePoint;

        [SerializeField]
        private ED_MapCom mapCom;

        public void Init(string uid, int id, string actorName, ActorType type, ED_MapCom mapCom)
        {
            this.UId = uid;
            this.Id = id;
            this.ActorName = actorName;
            this.Type = type;
            this.mapCom = mapCom;
            SetGoName();
        }

        private void Awake()
        {
            PathRoot = transform.Find("Paths");
            InteractivePoint = transform.Find("InteractivePoint");
            CheckUid();
        }

        private void OnEnable()
        {
            if (string.IsNullOrEmpty(stateName))
            {
                stateName = "Default";
            }
        }

        private void OnDrawGizmos()
        {
            //if (IsMainActor)
            //{
            //    GUI.color = Color.red;
            //    Gizmos.DrawSphere(transform.position, 0.05f);
            //    GUI.color = Color.white;
            //}
        }

        private void CheckUid()
        {
            if (mapCom == null)
            {
                return;
            }
            List<ED_ActorCom> actorComs = mapCom.GetActors();
            for (int i = 0; i < actorComs.Count; i++)
            {
                ED_ActorCom actorCom = actorComs[i];
                if (!actorCom.Equals(this))
                {
                    if (actorCom.UId == UId)
                    {
                        UId = mapCom.CalcActorUid();
                    }
                }
            }
        }

        public void SetGoName()
        {
            gameObject.name = string.Format($"{ActorName}_{Id}_{UId}");
        }

        public ED_ActorPathCom CreatePath()
        {
            Transform pathTrans = transform.Find("Prefab/Path");
            GameObject newGo = MapEditorHelper.CreateObj(pathTrans.gameObject, PathRoot.gameObject);
            ED_ActorPathCom pathCom = newGo.GetComponent<ED_ActorPathCom>();
            pathCom.pointGo = transform.Find("Prefab/PathPoint").gameObject;
            return newGo.GetComponent<ED_ActorPathCom>();
        }

        public override object ExportData()
        {
            ActorModel actorData = new ActorModel();
            actorData.uid = UId;
            actorData.id = Id;
            actorData.pos = HandlePos(transform.position);
            actorData.roate = HandlePos(transform.localEulerAngles);
            actorData.scale = HandlePos(transform.localScale);
            actorData.isActive = Type == ActorType.Player ? true : gameObject.activeSelf;
            actorData.type = Type;
            actorData.stateName = stateName;

            //路径
            ED_ActorPathCom[] pathComs = PathRoot.GetComponentsInChildren<ED_ActorPathCom>(true);
            if (pathComs != null)
            {
                for (int i = 0; i < pathComs.Length; i++)
                {
                    actorData.paths.Add((ActorPathModel)pathComs[i].ExportData());
                }
            }

            //交互点
            if (InteractivePoint.gameObject.activeSelf)
            {
                Vector2 offVect = new Vector2(InteractivePoint.position.x, InteractivePoint.position.y) - new Vector2(transform.position.x, transform.position.y);
                actorData.interactivePoint = new Vector2(offVect.x, offVect.y);
            }
            else
            {
                actorData.interactivePoint = Vector2.zero;
            }

            return actorData;
        }

        public static Vector3 HandlePos(Vector3 pos)
        {
            float x = (float)Math.Round(pos.x, 2);
            float y = (float)Math.Round(pos.y, 2);
            float z = (float)Math.Round(pos.z, 2);
            return new Vector3(x, y, z);
        }
    }
} 
#endif
