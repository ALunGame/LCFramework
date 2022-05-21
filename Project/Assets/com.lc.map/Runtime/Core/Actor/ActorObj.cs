using System.Collections;
using UnityEngine;
using LCConfig;
using LCToolkit;
using LCECS;
using UnityEditor;
using LCECS.Core;
using System;

namespace LCMap
{
    public enum DirType
    {
        Left,
        Right,
    }

    public class ActorObj : MonoBehaviour
    {
        private ActorModel model;

        public ActorModel Model { get => model;}

        /// <summary>
        /// 唯一Id
        /// </summary>
        public int Uid { get; private set; }

        /// <summary>
        /// 配置Id
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 实体配置Id
        /// </summary>
        public int EntityId { get; private set; }



        /// <summary>
        /// 实体配置Id
        /// </summary>
        [Header("表现状态名")]
        [ReadOnly]
        [SerializeField]
        private string DisplayStateName;

        /// <summary>
        /// 实体配置Id
        /// </summary>
        [Header("表现根节点")]
        [ReadOnly]
        [SerializeField]
        private GameObject DisplayRootGo;

        /// <summary>
        /// 实体配置Id
        /// </summary>
        [Header("表现节点")]
        [ReadOnly]
        [SerializeField]
        private GameObject DisplayGo;

        /// <summary>
        /// 实体配置Id
        /// </summary>
        [Header("相机跟随节点")]
        [ReadOnly]
        [SerializeField]
        private GameObject CameraFollowGo;

        /// <summary>
        /// 当表现节点改变
        /// </summary>
        public event Action<ActorObj> OnDisplayGoChange;

        private Entity entity;

        public void Init(ActorModel model)
        {
            Init(model, Config.ActorCnf[model.id].entityId);
        }

        public void Init(ActorModel model,int entityId)
        {
            this.model      = model;
            this.Uid        = Model.uid;
            this.Id         = Model.id;
            this.EntityId   = entityId;

            //表现根节点
            if (transform.Find("Display") == null)
            {
                this.DisplayRootGo = gameObject;
            }
            else
            {
                this.DisplayRootGo = transform.Find("Display").gameObject;
            }

            SetModelDisplay();
            UpdateGoName();

            entity = ECSLocate.ECS.CreateEntity(this);

            //设置玩家
            if (model.isMainActor)
            {
                ECSLocate.Player.SetPlayerEntity(entity);
            }
        }

        /// <summary>
        /// 设置显示
        /// </summary>
        public void SetModelDisplay()
        {
            //位置
            transform.position = model.pos;
            //旋转
            transform.localEulerAngles = model.roate;
            //缩放
            transform.localScale = model.scale;
            //显隐
            gameObject.SetActive(model.isActive);
            //显示节点
            SetDisplayGo(model.stateName);
        }

        public void Clear()
        {
            OnDisplayGoChange = null;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(GetInteractivePoint(),0.05f);

            foreach (var item in entity.GetComs())
            {
                item.OnDrawGizmosSelected();
            }
        }

        private void OnDrawGizmos()
        {
            foreach (var item in entity.GetComs())
            {
                item.OnDrawGizmos();
            }
        }

        #region Misc

        private void UpdateGoName()
        {
            if (!Config.ActorCnf.ContainsKey(Id))
                return;
            ActorCnf actorCnf = Config.ActorCnf[Id];
            this.name = string.Format("{0}-{1}-{2}", actorCnf.name, actorCnf.id,Uid);
        }

        #endregion

        #region Set

        public void SetDisplayGo(string stateName)
        {
            this.DisplayStateName = stateName;
            Transform displayTrans = DisplayRootGo.transform.Find(stateName);
            if (displayTrans == null)
            {
                Debug.LogError("设置表现节点失败>>>" + stateName);
                displayTrans = gameObject.transform;
            }
            else
            {
                //全隐藏
                Transform displayRoot = transform.Find("Display");
                for (int i = 0; i < displayRoot.childCount; i++)
                {
                    displayRoot.GetChild(i).gameObject.SetActive(false);
                }
            }
            DisplayGo = displayTrans.gameObject;
            DisplayGo.SetActive(true);

            //设置跟随
            Transform followTrans = DisplayGo.transform.Find("Camera_Follow");
            if (followTrans == null)
                followTrans = DisplayGo.transform;
            CameraFollowGo = followTrans.gameObject;

            OnDisplayGoChange?.Invoke(this);
        }

        public void SetDir(DirType dirType)
        {
            GameObject disGo = GetDisplayGo();
            Vector3 oldDir = disGo.transform.localEulerAngles;
            int yValue = dirType == DirType.Right ? 0 : 180;
            disGo.transform.localEulerAngles = new Vector3(oldDir.x, yValue, oldDir.z);
        }

        #endregion

        #region Get

        /// <summary>
        /// 表现节点
        /// </summary>
        /// <returns></returns>
        public GameObject GetDisplayRootGo()
        {
            return DisplayRootGo;
        }

        /// <summary>
        /// 表现节点
        /// </summary>
        /// <returns></returns>
        public GameObject GetDisplayGo()
        {
            return DisplayGo;
        }

        /// <summary>
        /// 获得相机跟随节点
        /// </summary>
        /// <returns></returns>
        public GameObject GetFollowGo()
        {
            return CameraFollowGo;
        }

        /// <summary>
        /// 方向
        /// </summary>
        /// <returns></returns>
        public DirType GetDir()
        {
            GameObject disGo = GetDisplayGo();
            return disGo.transform.localEulerAngles.y == 0 ? DirType.Right : DirType.Left;
        }

        /// <summary>
        /// 获得方向值
        /// </summary>
        /// <returns></returns>
        public int GetDirValue()
        {
            DirType dirType = GetDir();
            return dirType == DirType.Right ? 1 : -1;
        }

        /// <summary>
        /// 获得交互点
        /// </summary>
        /// <returns></returns>
        public Vector3 GetInteractivePoint()
        {
            DirType dirType = GetDir();
            Vector3 oldPos = Model.interactivePoint;
            float xValue = dirType == DirType.Right ? oldPos.y : -oldPos.y;

            Vector3 actorPos = transform.position;
            Vector3 pos = new Vector3(actorPos.x + xValue, actorPos.y + oldPos.y, actorPos.z + oldPos.z);
            return pos;
        }

        #endregion


    }
}