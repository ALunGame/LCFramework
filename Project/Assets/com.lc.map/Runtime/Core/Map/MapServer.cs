using System;
using LCConfig;
using System.Collections.Generic;
using UnityEngine;
using LCLoad;
using LCToolkit;
using Demo;
using LCECS.Core;

namespace LCMap
{
    public class MapServer
    {
        private GameObject mapRoot;
        /// <summary>
        /// 地图根节点
        /// </summary>
        public GameObject MapRoot
        {
            get
            {
                if (mapRoot == null)
                {
                    GameObject root = new GameObject("MapRoot");
                    root.transform.localPosition    = Vector3.zero;
                    root.transform.localRotation    = Quaternion.identity;
                    root.transform.localScale       = Vector3.one;
                    mapRoot = root;
                }
                return mapRoot;
            }
        }

        private GameObject playerRoot;
        /// <summary>
        /// 玩家根节点
        /// </summary>
        public GameObject PlayerRoot
        {
            get
            {
                if (playerRoot == null)
                {
                    GameObject root = new GameObject("PlayerRoot");
                    root.transform.localPosition    = Vector3.zero;
                    root.transform.localRotation    = Quaternion.identity;
                    root.transform.localScale       = Vector3.one;
                    playerRoot = root;
                }
                return playerRoot;
            }
        }

        public Actor PlayerActor { get; private set; }

        private int currMapId;
        /// <summary>
        /// 当前地图Id
        /// </summary>
        public int CurrMapId
        {
            get
            {
                return currMapId;
            }
        }

        private MapArea currArea;
        /// <summary>
        /// 当前地图区域
        /// </summary>
        public MapArea CurrArea
        {
            get
            {
                return currArea;
            }
        }

        private int currMaxActorUid;
        

        //地图区域
        public Dictionary<int, MapArea> areaDict = new Dictionary<int, MapArea>();
        //地图配置
        private Dictionary<int, MapInfo> mapCnf = new Dictionary<int, MapInfo>();

        private Action enterFinishCallBack;

        private MapInfo GetMapCnf(int pMapId)
        {
            if (mapCnf.ContainsKey(pMapId))
            {
                return mapCnf[pMapId];
            }
            string mapAssetName = ConfigDef.GetCnfNoExName("Map_" + pMapId);
            string jsonStr = LoadHelper.LoadString(mapAssetName);
            MapInfo model = LCJson.JsonMapper.ToObject<MapInfo>(jsonStr);
            mapCnf.Add(pMapId, model);
            return model;
        }

        public void Enter(int pMapId,Action pFinishCallBack)
        {
            MapInfo mapModel = GetMapCnf(pMapId);
            if (mapModel == null)
            {
                MapLocate.Log.LogError("进入地图失败,没有对应配置", pMapId);
                return;
            }

            this.currMapId = pMapId;
            this.currMaxActorUid = mapModel.currMaxActorUid;

            //区域
            for (int i = 0; i < mapModel.areas.Count; i++)
            {
                MapArea mapArea = new MapArea(mapModel.areas[i]);
                areaDict.Add(mapArea.Id, mapArea);
            }

            //创建主角所在区域
            MapArea area = GetPosArea(mapModel.mainActor.pos);
            CreateArea(area);
            currArea = area;
            LCECS.ECSLayerLocate.Info.GetSensor<GlobalSensor>(LCECS.SensorType.Global).CurrArea.Value = area;

            //创建主角
            CreateMainActor(mapModel.mainActor);

            enterFinishCallBack = pFinishCallBack;
            enterFinishCallBack?.Invoke();
        }

        public void Exit()
        {

        }

        private void CreateArea(MapArea area)
        {
            GameObject go = area.Create();
            go.transform.SetParent(MapRoot.transform);
            go.transform.Reset();
            go.transform.localPosition = area.Model.pos;
        }

        //获得坐标所属的区域
        public MapArea GetPosArea(Vector3 pos)
        {
            foreach (var item in areaDict)
            {
                if (item.Value.Rect.Contains(pos))
                {
                    return item.Value;
                }
            }
            return null;
        }

        //获得演员所在区域
        public MapArea GetAreaByActor(Actor pActor)
        {
            if (PlayerActor != null && PlayerActor.Uid == pActor.Uid)
            {
                return GetPosArea(PlayerActor.Pos);
            }
            foreach (var item in areaDict)
            {
                if (item.Value.GetActor(pActor.Uid)!=null)
                {
                    return item.Value;
                }
            }
            return null;
        }

        public Actor GetActor(string uid)
        {
            if (PlayerActor!=null && PlayerActor.Uid == uid)
            {
                return PlayerActor;
            }
            Actor actorObj = null;
            foreach (var item in areaDict)
            {
                actorObj = item.Value.GetActor(uid);
                if (actorObj != null)
                {
                    return actorObj;
                }
            }
            return actorObj;
        }

        public List<Actor> GetActors(int actorId)
        {
            List<Actor> actors = new List<Actor>();
            if (PlayerActor != null && PlayerActor.Id == actorId)
            {
                actors.Add(PlayerActor);
                return actors;
            }
            foreach (var item in areaDict)
            {
                foreach (var actor in item.Value.Actors.Values)
                {
                    if (actor.Id == actorId)
                    {
                        actors.Add(actor);
                    }
                }
            }
            return actors;
        }

        public Actor GetActor(int actorId)
        {
            List<Actor> actors = GetActors(actorId);
            if (actors.Count == 0)
                return null;
            return actors[0];
        }

        public IEnumerable<Actor> GetActors(string comTypeFullName)
        {
            if (PlayerActor.HasCom(comTypeFullName))
                yield return PlayerActor;

            foreach (var item in areaDict)
            {
                foreach (var actor in item.Value.Actors.Values)
                {
                    if (actor.HasCom(comTypeFullName))
                        yield return actor;
                }
            }
        }

        public Actor CreateActor(ActorInfo actorModel, MapArea mapArea)
        {
            actorModel.uid = CalcCreateActorUid();
            return mapArea.CreateActor(actorModel);
        }

        private void CreateMainActor(ActorInfo actor)
        {
            Actor tActor = ActorCreator.CreateActor(actor);
            tActor.Go.transform.SetParent(PlayerRoot.transform);

            //保存
            PlayerActor = tActor;
            LCECS.ECSLayerLocate.Info.GetSensor<GlobalSensor>(LCECS.SensorType.Global).FollowActor.Value = PlayerActor;
        }

        private string CalcCreateActorUid()
        {
            currMaxActorUid++;
            return "createActor" + currMaxActorUid.ToString();
        }
    }
}