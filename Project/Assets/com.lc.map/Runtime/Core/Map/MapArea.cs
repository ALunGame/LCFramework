using System.Collections.Generic;
using UnityEngine;
using LCLoad;
using LCToolkit;
using LCConfig;

namespace LCMap
{
    /// <summary>
    /// 地图区域
    /// </summary>
    public class MapArea
    {
        public int Id;

        public AreaModel Model;

        public Rect Rect;

        public GameObject AreaRootGo;
        public GameObject AreaEnvGo;

        public GameObject ActorRootGo;
        public Dictionary<int, ActorObj> Actors = new Dictionary<int, ActorObj>();

        public MapArea(AreaModel model)
        {
            this.Model = model;
            this.Id = model.areaId;
            this.Rect = model.rect;
        }

        public GameObject Create()
        {
            AreaRootGo = new GameObject("Area_"+Id);

            //预制体
            GameObject assetGo = LoadHelper.LoadPrefab(Model.areaPrefab);
            AreaEnvGo = GameObject.Instantiate(assetGo);
            AreaEnvGo.transform.SetParent(AreaRootGo.transform);
            AreaEnvGo.transform.Reset();

            //演员
            ActorRootGo = new GameObject("ActorRootGo");
            ActorRootGo.transform.SetParent(AreaRootGo.transform);
            ActorRootGo.transform.Reset();

            for (int i = 0; i < Model.actors.Count; i++)
            {
                ActorModel actorModel = Model.actors[i];
                CreateActorObj(actorModel);
            }

            return AreaRootGo;
        }

        private void CreateActorObj(ActorModel actor)
        {
            ActorCnf actorCnf = Config.ActorCnf[actor.id];

            //预制体
            GameObject assetGo = LoadHelper.LoadPrefab(actorCnf.prefab.ObjName);
            GameObject actorGo = GameObject.Instantiate(assetGo);
            actorGo.transform.SetParent(ActorRootGo.transform);

            //添加组件
            ActorObj actorObj = actorGo.AddComponent<ActorObj>();
            actorObj.Init(actor);

            //保存
            Actors.Add(actor.uid, actorObj);
        }

        public void Clear()
        {

        }
    }
}