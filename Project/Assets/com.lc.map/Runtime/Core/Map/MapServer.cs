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

        public ActorObj PlayerActor { get; private set; }

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

        //地图区域
        public Dictionary<int, MapArea> areaDict = new Dictionary<int, MapArea>();
        //地图配置
        private Dictionary<int, MapModel> mapCnf = new Dictionary<int, MapModel>();

        private MapModel GetMapCnf(int mapId)
        {
            if (mapCnf.ContainsKey(mapId))
            {
                return mapCnf[mapId];
            }
            string mapAssetName = ConfigDef.GetCnfNoExName("Map_" + mapId);
            string jsonStr = LoadHelper.LoadString(mapAssetName);
            MapModel model = LCJson.JsonMapper.ToObject<MapModel>(jsonStr);
            mapCnf.Add(mapId, model);
            return model;
        }

        public void Enter(int mapId)
        {
            MapModel mapModel = GetMapCnf(mapId);
            if (mapModel == null)
            {
                MapLocate.Log.LogError("进入地图失败,没有对应配置", mapId);
                return;
            }

            this.currMapId = mapId;

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

        public ActorObj GetActor(string uid)
        {
            if (PlayerActor!=null && PlayerActor.Uid == uid)
            {
                return PlayerActor;
            }
            ActorObj actorObj = null;
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

        public List<ActorObj> GetActors(int actorId)
        {
            List<ActorObj> actors = new List<ActorObj>();
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

        public ActorObj GetActor(int actorId)
        {
            List<ActorObj> actors = GetActors(actorId);
            if (actors.Count == 0)
                return null;
            return actors[0];
        }

        public IEnumerable<ActorObj> GetActors(string comTypeFullName)
        {
            if (PlayerActor.Entity.HasCom(comTypeFullName))
                yield return PlayerActor;

            foreach (var item in areaDict)
            {
                foreach (var actor in item.Value.Actors.Values)
                {
                    if (actor.Entity.HasCom(comTypeFullName))
                        yield return actor;
                }
            }
        }

        private void CreateMainActor(ActorModel actor)
        {
            ActorCnf actorCnf = Config.ActorCnf[actor.id];

            //预制体
            GameObject assetGo = LoadHelper.LoadPrefab(actorCnf.prefab.ObjName);
            GameObject actorGo = GameObject.Instantiate(assetGo);
            actorGo.transform.SetParent(PlayerRoot.transform);

            //添加组件
            PlayerActor = actorGo.AddComponent<ActorObj>();
            PlayerActor.Init(actor, currArea);

            //跟随
            LCECS.ECSLayerLocate.Info.GetSensor<GlobalSensor>(LCECS.SensorType.Global).FollowActor.Value = PlayerActor;
        }
    }
}