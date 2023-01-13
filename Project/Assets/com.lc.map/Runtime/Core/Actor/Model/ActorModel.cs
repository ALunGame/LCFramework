using Demo;
using LCECS.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LCMap
{
    /// <summary>
    /// 演员数据结构
    /// </summary>
    public class ActorModel
    {
        private string uid;

        /// <summary>
        /// 唯一Id
        /// </summary>
        public string Uid { get => uid;}

        /// <summary>
        /// 演员Id
        /// </summary>
        public int Id;

        /// <summary>
        /// 演员显示隐藏
        /// </summary>
        public bool IsActive;
        /// <summary>
        /// 演员状态名
        /// </summary>
        public string StateName;
        /// <summary>
        /// 演员类型
        /// </summary>
        public ActorType Type;

        //坐标
        public Vector3 Pos;
        //旋转
        public Vector3 Roate;
        //缩放
        public Vector3 Scale;

        //扩展参数
        public Dictionary<string, object> ExParam = new Dictionary<string, object>();

        public ActorModel(string uid)
        {
            this.uid = uid;
        }
    }

    public class Actor : Entity
    {
        public int Id;
        public string StateName { get { return displayCom.StateName; } }

        [NonSerialized]
        private BindGoCom bindGoCom;
        public BindGoCom BindGo { get { return bindGoCom; } }
        public GameObject Go { get { return bindGoCom.Go; } }

        [NonSerialized]
        private TransCom transCom;
        public TransCom Trans { get { return transCom; } }
        public Vector3 Pos { get { return transCom.Pos; } }
        public Vector3 Roate { get { return transCom.Roate; } }
        public Vector3 Scale { get { return transCom.Scale; } }

        [NonSerialized]
        private ActorDisplayCom displayCom;
        public ActorDisplayCom DisplayCom { get { return displayCom; } }

        [NonSerialized]
        private ActorInteractiveCom interactiveCom;
        public ActorInteractiveCom InteractiveCom { get { return interactiveCom; } }

        public Actor(string uid) : base(uid)
        {
        }

        protected override void OnInit()
        {
            bindGoCom       = GetOrCreateCom<BindGoCom>();
            transCom        = GetOrCreateCom<TransCom>();
            displayCom      = GetOrCreateCom<ActorDisplayCom>();
            interactiveCom  = GetOrCreateCom<ActorInteractiveCom>();
        }

        #region Interactive

        public bool Interactiving()
        {
            return interactiveCom.IsExecuting;
        }

        public string InteractivingKey()
        {
            return interactiveCom.ExecutingKey;
        }

        public void AddInteractive(ActorInteractive pInteractive)
        {
            interactiveCom.Add(pInteractive);
        }

        public void RemoveInteractive(ActorInteractive pInteractive)
        {
            interactiveCom.Remove(pInteractive);
        }

        public void ExecuteInteractive(Actor pActor, ActorInteractive pInteractive, Action<InteractiveState> pExecuteFinishCallBack = null, params object[] pParams)
        {
            interactiveCom.Execute(pActor, pInteractive, pExecuteFinishCallBack,pParams);
        }

        public void ExecuteInteractive(Actor pActor, InteractiveType pInteractiveType, Action<InteractiveState> pExecuteFinishCallBack = null, params object[] pParams)
        {
            ActorInteractive interactive = interactiveCom.Get(pInteractiveType);
            if (interactive != null)
            {
                ExecuteInteractive(pActor, interactive, pExecuteFinishCallBack,pParams);
            }
        }

        #endregion
    }
}
