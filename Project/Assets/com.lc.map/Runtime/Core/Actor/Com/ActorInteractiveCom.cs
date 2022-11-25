using LCECS.Core;
using System.Collections.Generic;
using System;
using Demo;

namespace LCMap
{
    public abstract class ActorInteractive
    {
        public abstract int Type { get;}

        protected Actor actor;
        protected ActorInteractiveCom interactiveCom;

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

        //判断交互是否可以执行
        public bool Evaluate(Actor pActor)
        {
            if (actor == null || actor.IsActive == false)
                return false;
            if (interactiveCom == null || interactiveCom.IsActive == false)
                return false;
            return OnEvaluate(pActor);
        }

        protected virtual bool OnEvaluate(Actor pActor)
        {
            return true;
        }

        public bool Execute(Actor pActor)
        {
            return OnExecute(pActor);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pActor"></param>
        /// <returns>返回是否完成交互</returns>
        protected virtual bool OnExecute(Actor pActor)
        {
            return false;
        }

        protected void Finish()
        {
            interactiveCom.ExecuteFinish();
        }
    }

    public class ActorInteractiveCom : BaseCom
    {
        [NonSerialized]
        private Entity entity;
        [NonSerialized]
        private Dictionary<int, Dictionary<string, ActorInteractive>> interactives = new Dictionary<int, Dictionary<string, ActorInteractive>>();
        [NonSerialized]
        private bool isExecuting = false;
        [NonSerialized]
        private string executingKey = "";

        public bool IsExecuting { get { return isExecuting; } }
        public string ExecutingKey { get { return executingKey; } }

        protected override void OnInit(Entity entity)
        {
            base.OnInit(entity);
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

        public void Execute(Actor pActor, ActorInteractive pInteractive)
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
            if (tInteractive.Execute(pActor))
                ExecuteFinish();
        }

        public void ExecuteFinish()
        {
            isExecuting = false;
            executingKey = "";
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