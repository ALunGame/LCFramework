using LCECS.Core;
using LCECS.Data;
using LCECS.Help;
using System;
using System.Collections.Generic;
using UnityEngine;
using LCLoad;
using LCJson;
using LCToolkit;
using LCMap;

namespace LCECS.Server.ECS
{
    /// <summary>
    /// 1，保存所有的实体
    /// 2，所有的Update系统
    /// 3，所有的FixedUpdate系统
    /// 4，提供创建实体，执行系统，获得实体方法
    /// </summary>
    public class ECSServer : IECSServer
    {
        private Dictionary<int, string> entityCnf = new Dictionary<int, string>();

        //全局单个组件
        private Dictionary<Type, BaseCom> globalSingleCom = new Dictionary<Type, BaseCom>();
        private Dictionary<Type, Action> globalSingleComCallBack = new Dictionary<Type, Action>();

        //实体列表
        private Dictionary<int, Entity> entityDict = new Dictionary<int, Entity>();

        //所有Update系统
        private List<BaseSystem> systemUpdateList = new List<BaseSystem>();

        //所有FixedUpdate系统
        private List<BaseSystem> systemFixedUpdateList = new List<BaseSystem>();

        private string GetEntityCnf(int entityConfId)
        {
            if (entityCnf.ContainsKey(entityConfId))
            {
                return entityCnf[entityConfId];
            }
            string jsonStr = LoadHelper.LoadString(ECSDefPath.GetEntityCnfName(entityConfId));
            entityCnf.Add(entityConfId, jsonStr);
            return jsonStr;
        }

        //添加实体
        private void AddEntity(int id, Entity entity)
        {
            if (!entityDict.ContainsKey(id))
                entityDict.Add(id, entity);
        }

        //检测系统是否检测该实体
        private void CheckEntityInSystem(Entity entity)
        {
            for (int i = 0; i < systemUpdateList.Count; i++)
            {
                systemUpdateList[i].CheckEntity(entity);
            }

            for (int i = 0; i < systemFixedUpdateList.Count; i++)
            {
                systemFixedUpdateList[i].CheckEntity(entity);
            }
        }

        //初始化实体（所有的实体生成都会走）
        private void InitEntity(Entity entity)
        {
            //保存
            AddEntity(entity.Uid, entity);

            //系统检测
            CheckEntityInSystem(entity);

            //创建实体数据流
            EntityWorkData entityWorkData = new EntityWorkData(entity.Uid, entity);
            entityWorkData.Id = entity.Uid;
            ECSLayerLocate.Info.AddEntityWorkData(entity.Uid, entityWorkData);
            ECSLayerLocate.Decision.AddDecisionEntity(entity.DecTreeId, entityWorkData);

            //添加实体全局单一组件
            AddEntityGlobalSingleCom(entity);
        }

        //添加实体全局单一组件
        private void AddEntityGlobalSingleCom(Entity entity)
        {
            foreach (var item in entity.GetComs())
            {
                if (ECSHelp.CheckComIsGlobal(item.GetType()))
                {
                    if (globalSingleCom.ContainsKey(item.GetType()))
                    {
                        ECSLocate.Log.LogError("有多个全局单个组件>>>>>>", entity.Id);
                        entity.Disable();
                        return;
                    }
                    globalSingleCom.Add(item.GetType(), item);
                }
            }
        }

        //-------------------------------------------------------- 接口实现 --------------------------------------------------------//

        public Entity CreateEntity(ActorObj actorObj)
        {
            //配置数据
            string entityStr = GetEntityCnf(actorObj.EntityId);
            if (string.IsNullOrEmpty(entityStr))
            {
                ECSLocate.Log.LogError("实体配置数据不存在>>>>>>>", actorObj.EntityId);
                return null;
            }

            //创建实体
            Entity entity = JsonMapper.ToObject<Entity>(entityStr);
            entity.SetEntityGo(actorObj.gameObject);
            entity.Init(actorObj.Uid);
            foreach (BaseCom com in entity.GetComs())
            {
                com.Init(entity);
            }

            InitEntity(entity);
            return entity;
        }

        public Entity CreateEntity(int uid,int id, GameObject go)
        {
            //配置数据
            string entityStr = GetEntityCnf(id);
            if (string.IsNullOrEmpty(entityStr))
            {
                ECSLocate.Log.LogError("实体配置数据不存在>>>>>>>", id);
                return null;
            }

            //创建实体
            Entity entity = JsonMapper.ToObject<Entity>(entityStr);
            entity.SetEntityGo(go);
            entity.Init(uid);
            foreach (BaseCom com in entity.GetComs())
            {
                com.Init(entity);
            }

            InitEntity(entity);

            return entity;
        }

        //获得实体
        public Entity GetEntity(int uid)
        {
            return entityDict[uid];
        }

        public Dictionary<int, Entity> GetAllEntitys()
        {
            return entityDict;
        }

        public void CheckEntityInSystem(int uid)
        {
            Entity entity = GetEntity(uid);
            if (entity == null)
                return;
            CheckEntityInSystem(entity);
        }

        //获得全局单一组件
        public T GetGlobalSingleCom<T>() where T : BaseCom
        {
            Type comType = typeof(T);
            if (!globalSingleCom.ContainsKey(comType))
            {
                ECSLocate.Log.LogError("获得全局单一组件，出错 没有该全局组件>>>>>>", comType);
                return null;
            }
            return (T)globalSingleCom[comType];
        }

        //设置全局单一组件的值
        public void SetGlobalSingleComData<T>(Action<T> changeData) where T : BaseCom
        {
            Type comType = typeof(T);
            if (!globalSingleCom.ContainsKey(comType))
            {
                ECSLocate.Log.LogError("设置全局单一组件的值，出错 没有该全局组件>>>>>>", comType);
                return;
            }
            //回调
            changeData((T)globalSingleCom[comType]);
            //广播事件
            Action callBack = null;
            if (globalSingleComCallBack.ContainsKey(comType))
                callBack = globalSingleComCallBack[comType];
            callBack?.Invoke();
        }

        public void RegGlobalSingleComChangeCallBack(Type comType, Action callBack)
        {
            if (globalSingleComCallBack.ContainsKey(comType))
            {
                globalSingleComCallBack[comType] += callBack;
            }
            else
            {
                globalSingleComCallBack[comType] = callBack;
            }
        }

        public void RegUpdateSystem(BaseSystem system)
        {
            if (systemUpdateList.Contains(system))
                return;
            systemUpdateList.Add(system);
        }

        public void RegFixedUpdateSystem(BaseSystem system)
        {
            if (systemFixedUpdateList.Contains(system))
                return;
            systemFixedUpdateList.Add(system);
        }

        public void ExcuteUpdateSystem()
        {
            for (int i = 0; i < systemUpdateList.Count; i++)
            {
                systemUpdateList[i].Excute();
            }
        }

        public void ExcuteFixedUpdateSystem()
        {
            for (int i = 0; i < systemFixedUpdateList.Count; i++)
            {
                systemFixedUpdateList[i].Excute();
            }
        }
    }
}
