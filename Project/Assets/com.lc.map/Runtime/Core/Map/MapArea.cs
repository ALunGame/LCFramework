using System.Collections.Generic;
using UnityEngine;
using LCLoad;
using LCToolkit;
using LCConfig;
using Cinemachine;

namespace LCMap
{
    /// <summary>
    /// 地图区域
    /// </summary>
    public class MapArea
    {
        public int Id;

        public AreaInfo Model;

        public Rect Rect;

        public GameObject AreaRootGo;
        public GameObject AreaEnvGo;

        public GameObject ActorRootGo;
        public Dictionary<string, Actor> Actors = new Dictionary<string, Actor>();

        public CinemachineVirtualCamera FollowCamera;
        public CinemachineVirtualCamera DragCamera;
        public GameObject DragTarget;
        public Transform CameraColliderRootTrans;

        public MapArea(AreaInfo model)
        {
            this.Id = model.areaId;
            this.Model = model;
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

            InitArea();

            //演员根节点
            ActorRootGo = new GameObject("ActorRootGo");
            ActorRootGo.transform.SetParent(AreaRootGo.transform);
            ActorRootGo.transform.Reset();

            for (int i = 0; i < Model.actors.Count; i++)
            {
                ActorInfo actorModel = Model.actors[i];
                CreateActor(actorModel);
            }
            return AreaRootGo;
        }

        private void InitArea()
        {
            Transform camRootTrans = AreaEnvGo.transform.Find("CMCamera");
            if (camRootTrans == null)
            {
                MapLocate.Log.LogError("地图区域创建失败，没有相机节点>>>", Id);
                return;
            }
            FollowCamera = camRootTrans.Find("FollowCamera").GetComponent<CinemachineVirtualCamera>();
            DragCamera = camRootTrans.Find("DragCamera").GetComponent<CinemachineVirtualCamera>();
            DragTarget = camRootTrans.Find("DragTarget").gameObject;
            CameraColliderRootTrans = camRootTrans.Find("CameraColliders");
        }

        public Actor CreateActor(ActorInfo actor)
        {
            Actor tActor = ActorCreator.CreateActor(actor);
            //保存
            Actors.Add(actor.uid, tActor);
            tActor.Go.transform.SetParent(ActorRootGo.transform);
            return tActor;
        }

        public Actor GetActor(string uid)
        {
            if (!Actors.ContainsKey(uid))
            {
                return null;
            }
            return Actors[uid];
        }

        public void Clear()
        {

        }

        #region 相机

        /// <summary>
        /// 设置相机范围
        /// </summary>
        /// <param name="colliderName">相机范围名</param>
        public void SetCameraCollider(string colliderName = "")
        {
            if (string.IsNullOrEmpty(colliderName))
                colliderName = "DefaultCollider";
            Transform colliderTrans = CameraColliderRootTrans.Find(colliderName);
            if (colliderTrans == null)
            {
                MapLocate.Log.LogError("设置相机范围失败，没有节点>>>", Id,colliderName);
                return;
            }
            Collider2D collider2D = colliderTrans.GetComponent<Collider2D>();
            FollowCamera.GetComponent<CinemachineConfiner>().m_BoundingShape2D = collider2D;
            DragCamera.GetComponent<CinemachineConfiner>().m_BoundingShape2D = collider2D;
        }

        /// <summary>
        /// 获得相机范围
        /// </summary>
        /// <returns></returns>
        public PolygonCollider2D GetCameraCollider()
        {
            return (PolygonCollider2D)FollowCamera.GetComponent<CinemachineConfiner>().m_BoundingShape2D;
        }

        #endregion
    }
}