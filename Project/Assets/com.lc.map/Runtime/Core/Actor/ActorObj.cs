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
        [Header("唯一Id")]
        [EDReadOnly]
        public int Uid;

        /// <summary>
        /// 配置Id
        /// </summary>
        [Header("配置Id")]
        [EDReadOnly]
        public int Id;

        /// <summary>
        /// 实体配置Id
        /// </summary>
        [Header("实体配置Id")]
        [EDReadOnly]
        public int EntityId;

        /// <summary>
        /// 实体配置Id
        /// </summary>
        [Header("表现根节点")]
        [EDReadOnly]
        public GameObject DisplayRootGo;

        /// <summary>
        /// 实体配置Id
        /// </summary>
        [Header("表现状态名")]
        [EDReadOnly]
        public string DisplayStateName;

        /// <summary>
        /// 实体配置Id
        /// </summary>
        [Header("表现节点")]
        [EDReadOnly]
        public GameObject DisplayGo;

        /// <summary>
        /// 当表现节点改变
        /// </summary>
        public event Action<GameObject> OnDisplayGoChange;


        private Entity entity;
        public void Init(ActorModel model)
        {
            this.model = model;
            this.Uid = Model.uid;
            this.Id = Model.id;
            this.EntityId = Config.ActorCnf[Id].entityId;
            this.DisplayRootGo = transform.Find("Display").gameObject;
            SetModelDisplay();
            UpdateGoName();

            entity = ECSLocate.ECS.CreateEntity(this);

            //设置玩家
            if (model.isMainActor)
            {
                ECSLocate.Player.SetPlayerEntity(entity);
            }
        }

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

        #region Misc

        private void UpdateGoName()
        {
            ActorCnf actorCnf = Config.ActorCnf[Id];
            this.name = string.Format("{0}-{1}-{2}", actorCnf.name, actorCnf.id,Uid);
        }

        #endregion

        #region Get

        public GameObject GetDisplayGo()
        {
            return DisplayGo;
        }

        public DirType GetDir()
        {
            GameObject disGo = GetDisplayGo();
            return disGo.transform.localEulerAngles.y == 0 ? DirType.Right : DirType.Left;
        }

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

        #region Set

        public void SetDisplayGo(string stateName)
        {
            this.DisplayStateName = stateName;
            Transform displayTrans = transform.Find("Display/"+ stateName);
            if (displayTrans == null)
            {
                Debug.LogError("设置表现节点失败>>>" + stateName);
                DisplayGo = gameObject;
                return;
            }

            //全隐藏
            Transform displayRoot = transform.Find("Display");
            for (int i = 0; i < displayRoot.childCount; i++)
            {
                displayRoot.GetChild(i).gameObject.SetActive(false);
            }

            DisplayGo = displayTrans.gameObject;
            DisplayGo.SetActive(true);
            OnDisplayGoChange?.Invoke(DisplayGo);
        }

        public void SetDir(DirType dirType)
        {
            GameObject disGo = GetDisplayGo();
            Vector3 oldDir = disGo.transform.localEulerAngles;
            int yValue = dirType == DirType.Right ? 0 : 180;
            disGo.transform.localEulerAngles = new Vector3(oldDir.x, yValue, oldDir.z);
        }

        #endregion
    }
}