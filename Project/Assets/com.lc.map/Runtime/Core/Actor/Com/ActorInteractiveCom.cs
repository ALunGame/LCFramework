using LCECS.Core;
using System.Collections.Generic;
using System;
using Demo;

namespace LCMap
{
    /// <summary>
    /// 交互状态
    /// </summary>
    public enum InteractiveState
    {
        /// <summary>
        /// 正在执行
        /// </summary>
        Executing,
        /// <summary>
        /// 交互成功
        /// </summary>
        Success,
        /// <summary>
        /// 交互失败
        /// </summary>
        Fail,
    }
    
    public abstract class ActorInteractive
    {
        public abstract int Type { get;}

        protected Actor actor;
        protected ActorInteractiveCom interactiveCom;

        public InteractiveState State { get; private set; }

        //创建交互
        public void Create(Actor pActor, ActorInteractiveCom pInteractiveCom)
        {
            actor = pActor;
            interactiveCom = pInteractiveCom;
            OnCreate(pActor);
        }

        protected virtual void OnCreate(Actor pActor)
        {

        }

        //交互标识
        public string GetHasKey()
        {
            string hasKey = actor.Uid + Type + OnGetHasKey();
            return hasKey;
        }

        protected virtual string OnGetHasKey()
        {
            return "";
        }

        //删除交互
        public void Remove()
        {
            OnRemove();
        }

        protected virtual void OnRemove()
        {

        }

        /// <summary>
        /// 判断交互是否可以执行
        /// </summary>
        /// <param name="pInteractiveActor">交互的演员</param>
        /// <returns></returns>
        public bool Evaluate(Actor pInteractiveActor)
        {
            if (actor == null || actor.IsActive == false)
                return false;
            if (interactiveCom == null || interactiveCom.IsActive == false)
                return false;
            return OnEvaluate(pInteractiveActor);
        }

        /// <summary>
        /// 判断交互是否可以执行
        /// </summary>
        /// <param name="pInteractiveActor">交互的演员</param>
        /// <returns></returns>
        protected virtual bool OnEvaluate(Actor pInteractiveActor)
        {
            return true;
        }

        /// <summary>
        /// 执行交互
        /// </summary>
        /// <param name="pInteractiveActor">交互的演员</param>
        /// <returns></returns>
        public void Execute(Actor pInteractiveActor, params object[] pParams)
        {
            State = OnExecute(pInteractiveActor,pParams);
            if (State == InteractiveState.Success)
            {
                Success();
            }
            else if (State == InteractiveState.Fail)
            {
                Fail();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pInteractiveActor">交互的演员</param>
        /// <param name="pParams"></param>
        /// <returns>返回是否完成交互</returns>
        protected virtual InteractiveState OnExecute(Actor pInteractiveActor,params object[] pParams)
        {
            return InteractiveState.Success;
        }

        /// <summary>
        /// 交互成功
        /// </summary>
        protected void Success()
        {
            State = InteractiveState.Success;
            interactiveCom.ExecuteFinish(State);
        }

        /// <summary>
        /// 交互失败
        /// </summary>
        protected void Fail()
        {
            State = InteractiveState.Fail;
            interactiveCom.ExecuteFinish(State);
        }
    }

    public class ActorInteractiveCom : BaseCom
    {
        /// <summary>
        /// 交互范围
        /// </summary>
        public float interactiveDis = 2;

        [NonSerialized]
        private Entity entity;
        [NonSerialized]
        private Dictionary<int, Dictionary<string, ActorInteractive>> interactives = new Dictionary<int, Dictionary<string, ActorInteractive>>();
        [NonSerialized]
        private bool isExecuting = false;
        [NonSerialized]
        private string executingKey = "";
        [NonSerialized]
        private Action<InteractiveState> executeFinishCallBack;

        public bool IsExecuting { get { return isExecuting; } }
        public string ExecutingKey { get { return executingKey; } }

        protected override void OnInit(Entity entity)
        {
            this.entity = entity;   
        }

        public void Add(ActorInteractive pInteractive)
        {
            if (pInteractive == null)
                return;

            //创建
            pInteractive.Create((Actor)entity,this);
            string hasKey = pInteractive.GetHasKey();

            //保存
            if (!interactives.ContainsKey(pInteractive.Type))
                interactives.Add(pInteractive.Type, new Dictionary<string, ActorInteractive>());
            if (interactives[pInteractive.Type].ContainsKey(hasKey))
            {
                MapLocate.Log.LogError("添加交互出错，交互重复", hasKey);
                return;
            }
            interactives[pInteractive.Type].Add(hasKey, pInteractive);
        }

        public void Remove(ActorInteractive pInteractive)
        {
            int type = pInteractive.Type;
            string hasKey = pInteractive.GetHasKey();
            if (!interactives.ContainsKey(type))
                return;

            if (!interactives[type].ContainsKey(hasKey))
                return;

            //删除
            interactives[type].Remove(hasKey);
            pInteractive.Remove();
        }

        public ActorInteractive Get(string hasKey)
        {
            foreach (var item in interactives)
            {
                foreach (var tValue in item.Value.Values)
                {
                    if (tValue.GetHasKey() == hasKey)
                    {
                        return tValue;
                    }
                }
            }
            return null;
        }

        public ActorInteractive Get(InteractiveType pInteractiveType)
        {
            foreach (var item in interactives)
            {
                foreach (var tValue in item.Value.Values)
                {
                    if (tValue.Type == (int)pInteractiveType)
                    {
                        return tValue;
                    }
                }
            }
            return null;
        }

        public void Execute(Actor pActor, ActorInteractive pInteractive, Action<InteractiveState> pExecuteFinishCallBack = null, params object[] pParams)
        {
            if (isExecuting)
            {
                MapLocate.Log.LogError("执行交互出错，正在执行交互", executingKey);
                return;
            }
            ActorInteractive tInteractive = Get(pInteractive.GetHasKey());
            if (tInteractive == null)
            {
                MapLocate.Log.LogError("执行交互出错，没有对应的交互", pInteractive.GetHasKey());
                return;
            }
            if (!tInteractive.Evaluate(pActor))
            {
                MapLocate.Log.LogWarning("执行交互出错，该交互无法执行", pInteractive.GetHasKey());
                return;
            }
            isExecuting = true;
            executingKey = pInteractive.GetHasKey();
            executeFinishCallBack = pExecuteFinishCallBack;
            tInteractive.Execute(pActor,pParams);
        }

        public void ExecuteFinish(InteractiveState pState)
        {
            isExecuting = false;
            executingKey = "";
            Action<InteractiveState> func = executeFinishCallBack;
            executeFinishCallBack = null;
            func?.Invoke(pState);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            List<ActorInteractive> collectInteractives = new List<ActorInteractive>();
            foreach (var item in interactives)
            {
                foreach (var tValue in item.Value.Values)
                {
                    collectInteractives.Add(tValue);
                }
            }
            for (int i = 0; i < collectInteractives.Count; i++)
            {
                Remove(collectInteractives[i]);
            }
        }
    }
}