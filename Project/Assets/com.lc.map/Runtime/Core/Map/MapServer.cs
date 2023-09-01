using System;
using LCConfig;
using System.Collections.Generic;
using UnityEngine;
using LCToolkit;
using Demo;
using LCECS.Core;

namespace LCMap
{
    public class MapServer : BaseServer
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

        private int currMaxActorUid;
        

        //地图区域
        public Dictionary<int, MapArea> areaDict = new Dictionary<int, MapArea>();
        public Dictionary<int, MapArea> AreaDict
        {
            get => areaDict;
        }
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
            string jsonStr = IAFramework.GameContext.Asset.LoadString(mapAssetName);
            MapInfo model = LCJson.JsonMapper.ToObject<MapInfo>(jsonStr);
            mapCnf.Add(pMapId, model);
            return model;
        }

        public MapServer()
        {
            SetLogicMapping(new MapServerLogicMapping());
        }

        #region 流程

        public void Enter(int pMapId,Action pFinishCallBack)
        {
            MapInfo mapModel = GetMapCnf(pMapId);
            if (mapModel == null)
            {
                MapLocate.Log.LogError("进入地图失败,没有对应配置", pMapId);
                return;
            }

            //数据
            this.currMapId = pMapId;
            this.currMaxActorUid = mapModel.currMaxActorUid;
            this.enterFinishCallBack = pFinishCallBack;
            
            //初始化
            Init();
        }

        public override void OnInit()
        {
            MapInfo mapModel = GetMapCnf(currMapId);

            //区域
            for (int i = 0; i < mapModel.areas.Count; i++)
            {
                MapArea mapArea = new MapArea(mapModel.areas[i]);
                areaDict.Add(mapArea.Id, mapArea);
            }

            //创建主角所在区域
            MapArea area = GetPosArea(mapModel.mainActor.pos);
            CreateArea(area);
            LCECS.ECSLayerLocate.Info.GetSensor<GlobalSensor>(LCECS.SensorType.Global).CurrArea.Value = area;

            //创建主角
            CreateMainActor(mapModel.mainActor);
            
            //进入区域
            area.EnterArea(ActorMediator.GetMainActor());
        }

        public void Exit()
        {
            Clear();
        }

        public override void OnClear()
        {
            base.OnClear();
        }

        #endregion
        
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
            return GetPosArea(pActor.Pos);
        }

        public Actor CreateActor(ActorInfo actorModel, MapArea mapArea)
        {
            actorModel.uid = CalcCreateActorUid();
            return mapArea.CreateActor(actorModel);
        }

        private void CreateMainActor(ActorInfo actor)
        {
            Actor tActor = ActorLocate.Actor.AddActor(actor);
            tActor.Go.transform.SetParent(PlayerRoot.transform);

            //保存
            ActorLocate.Actor.SetMainActor(tActor);
            LCECS.ECSLocate.Player.SetPlayerEntity(tActor);
            LCECS.ECSLayerLocate.Info.GetSensor<GlobalSensor>(LCECS.SensorType.Global).FollowActor.Value = tActor;
        }

        private string CalcCreateActorUid()
        {
            currMaxActorUid++;
            return "createActor_" + currMaxActorUid.ToString();
        }
    }
}