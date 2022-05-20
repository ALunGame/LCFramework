using LCECS.Core;
using LCECS.Data;
using LCJson;
using LCLoad;
using LCMap;
using System.Collections.Generic;
using UnityEngine;

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
        //实体配置
        private Dictionary<int, string> entityCnf = new Dictionary<int, string>();

        //世界实体
        private Entity world;

        //实体列表
        private Dictionary<int, Entity> entityDict = new Dictionary<int, Entity>();

        //所有Update系统
        private List<BaseSystem> systemUpdateList = new List<BaseSystem>();

        //所有FixedUpdate系统
        private List<BaseSystem> systemFixedUpdateList = new List<BaseSystem>();

        //获得实体配置
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
        }

        public void Init()
        {
            
        }

        #region Entity

        public Entity GetWorld()
        {
            if (world == null)
            {
                ActorModel worldActor = new ActorModel();
                worldActor.uid = -999;
                worldActor.id = -999;

                GameObject worldGo = new GameObject("<------------EntityWorld---------->");
                ActorObj actorObj = worldGo.AddComponent<ActorObj>();
                actorObj.Init(worldActor, -999);

                world = GetEntity(-999);
            }
            return world;
        }

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

        public Entity CreateEntity(int uid, int id, GameObject go)
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

        #endregion

        #region System

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


        #endregion

        public void Clear()
        {
            entityCnf.Clear();
        }
    }
}
