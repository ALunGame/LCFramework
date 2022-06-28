using LCConfig;
using LCECS;
using LCECS.Core;
using LCToolkit;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LCMap
{
    public enum DirType
    {
        Left,
        Right,
    }

    /// <summary>
    /// 演员类型
    /// </summary>
    public enum ActorType
    {
        /// <summary>
        /// 村民
        /// </summary>
        Villager,
        /// <summary>
        /// 动物
        /// </summary>
        Animal,
        /// <summary>
        /// 怪物
        /// </summary>
        Monster,
        /// <summary>
        /// 玩家
        /// </summary>
        Player,
        /// <summary>
        /// 物品
        /// </summary>
        Item,
        /// <summary>
        /// 建筑 
        /// </summary>
        Building,
    }

    public class ActorObj : MonoBehaviour
    {
        private ActorModel model;

        public ActorModel Model { get => model;}

        /// <summary>
        /// 所属的区域
        /// </summary>
        public MapArea Area { get; private set; }

        /// <summary>
        /// 唯一Id
        /// </summary>
        public string Uid { get; private set; }

        /// <summary>
        /// 配置Id
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 演员类型
        /// </summary>
        public ActorType Type { get; private set; }

        /// <summary>
        /// 实体配置Id
        /// </summary>
        public int EntityId { get; private set; }



        [Header("表现状态名")]
        [ReadOnly]
        [SerializeField]
        private string DisplayStateName;

        [Header("状态节点")]
        [ReadOnly]
        [SerializeField]
        private GameObject StateGo;

        [Header("表现节点")]
        [ReadOnly]
        [SerializeField]
        private GameObject DisplayGo;

        [Header("相机跟随节点")]
        [ReadOnly]
        [SerializeField]
        private GameObject CameraFollowGo;

        /// <summary>
        /// 当表现节点改变
        /// </summary>
        public event Action<ActorObj> OnDisplayGoChange;

        private Entity entity;

        public Entity Entity { get => entity;}

        public void Init(ActorModel model,MapArea mapArea)
        {
            Area = mapArea;
            ActorCnf actorCnf = Config.ActorCnf[model.id];
            foreach (var item in actorCnf.interactions)
            {
                AddInteractive(item);
            }
            Init(model, actorCnf.entityId);
        }

        public void Init(ActorModel model,int entityId)
        {
            this.model      = model;
            this.Uid        = Model.uid;
            this.Id         = Model.id;
            this.EntityId   = entityId;

            SetModelDisplay();
            UpdateGoName();

            entity = ECSLocate.ECS.CreateEntity(this);

            //设置玩家
            if (model.type == ActorType.Player)
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

        #region 交互

        /// <summary>
        /// 交互
        /// </summary>
        public List<ActorInteractive> Interactions { get; private set; }

        /// <summary>
        /// 当前正在执行的交互
        /// </summary>
        public ActorInteractive CurrInteractive { get; private set; }

        /// <summary>
        /// 添加交互
        /// </summary>
        /// <param name="interactive"></param>
        public void AddInteractive(ActorInteractive interactive)
        {
            if (Interactions == null)
                Interactions = new List<ActorInteractive>();
            foreach (var item in Interactions)
            {
                if (item.GetType() == interactive.GetType())
                {
                    MapLocate.Log.LogError("添加交互出错,重复的交互",interactive.GetType());
                    return;
                }
            }
            interactive.Init(this);
            Interactions.Add(interactive);
        }

        /// <summary>
        /// 删除交互
        /// </summary>
        /// <param name="interactive"></param>
        public void RemoveInteractive(ActorInteractive interactive)
        {
            if (Interactions == null)
                return;
            Interactions.Remove(interactive);
        }

        /// <summary>
        /// 执行交互
        /// </summary>
        /// <param name="interactive"></param>
        public void ExecuteInteractive<T>(ActorObj executeActor) where T : ActorInteractive
        {
            ExecuteInteractive(typeof(T), executeActor);    
        }

        /// <summary>
        /// 执行交互
        /// </summary>
        /// <param name="interactive"></param>
        public void ExecuteInteractive(Type type, ActorObj executeActor)
        {
            if (CurrInteractive != null)
            {
                MapLocate.Log.LogError("执行交互出错,当前正常执行交互", CurrInteractive.GetType());
                return;
            }
            if (Interactions == null)
                return;
            foreach (var item in Interactions)
            {
                if (item.GetType() == type)
                {
                    item.Execute(executeActor);
                    return;
                }
            }
            MapLocate.Log.LogError("执行交互出错,没有该交互", type);
        }

        public void SetCurrInteractive(ActorInteractive interactive)
        {
            CurrInteractive = interactive;
        }

        #endregion

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
            //全部隐藏
            Transform stateRoot = transform.Find("State");
            if (stateRoot != null)
            {
                for (int i = 0; i < stateRoot.childCount; i++)
                {
                    stateRoot.GetChild(i).gameObject.SetActive(false);
                }
            }

            Transform stateTrans = null;
            if (stateRoot == null)
            {
                stateTrans = transform;
            }
            else
            {
                if (string.IsNullOrEmpty(stateName))
                    stateTrans = stateRoot.GetChild(0);
                else
                {
                    stateTrans = stateRoot.Find(stateName);
                }
            }

            if (stateTrans == null)
            {
                stateTrans = transform;
                MapLocate.Log.LogError("设置状态节点出错>>>", Id, stateName);
            }

            DisplayStateName = stateTrans.name;

            DisplayGo = stateTrans.gameObject;
            if (stateTrans.Find("Display") != null)
                DisplayGo = stateTrans.Find("Display").gameObject;
            stateTrans.gameObject.SetActive(true);

            //设置跟随
            Transform followTrans = stateTrans.transform.Find("Camera_Follow");
            if (followTrans == null)
                followTrans = transform;
            CameraFollowGo = followTrans.gameObject;

            StateGo = stateTrans.gameObject;

            //更新碰撞
            UpdateCollider();
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
        /// 状态节点
        /// </summary>
        /// <returns></returns>
        public GameObject GetStateGo()
        {
            return StateGo;
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

        #region Collider

        private PolygonCollider2D clickCollider;
        private BoxCollider2D bodyCollider;

        private void UpdateCollider()
        {
            if (DisplayGo.transform.Find("ClickBox") != null)
            {
                clickCollider = DisplayGo.transform.Find("ClickBox").GetComponent<PolygonCollider2D>();
            }
            if (DisplayGo.transform.Find("BodyCollider") != null)
            {
                bodyCollider = DisplayGo.transform.Find("BodyCollider").GetComponent<BoxCollider2D>();
            }
        }

        public PolygonCollider2D GetClickCollider()
        {
            return clickCollider;
        }

        public BoxCollider2D GetBodyCollider()
        {
            return bodyCollider;
        }

        #endregion

    }
}