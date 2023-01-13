using Demo;
using LCECS.Core;
using LCECS.Data;
using LCJson;
using LCLoad;
using LCMap;
using System;
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
        private Dictionary<int, string> entityComsCnf = new Dictionary<int, string>();

        //世界实体
        private Entity world;

        //实体列表
        private Dictionary<string, Entity> entityDict = new Dictionary<string, Entity>();

        //系统
        private List<BaseSystem> systemList = new List<BaseSystem>();

        //检测系统是否检测该实体
        private void CheckEntityInSystem(Entity entity)
        {
            for (int i = 0; i < systemList.Count; i++)
            {
                systemList[i].CheckEntity(entity);
            }
        }

        public void Init()
        {
            
        }

        #region Entity

        public void SetWorld(Entity pWorld)
        {
            world = pWorld;
        }

        public Entity GetWorld()
        {
            if (world == null)
            {
                ActorInfo worldActor = new ActorInfo();
                worldActor.uid = "world_999";
                worldActor.id  = -999;

                world = ActorCreator.CreateActor(worldActor,new GameObject("<------------EntityWorld---------->"));
            }
            return world;
        }

        public List<BaseCom> GetEntityComsModel(int pCnfId)
        {
            string jsonStr;
            if (entityComsCnf.ContainsKey(pCnfId))
                jsonStr = entityComsCnf[pCnfId];
            else
            {
                jsonStr = LoadHelper.LoadString(ECSDefPath.GetEntityCnfName(pCnfId));
                entityComsCnf.Add(pCnfId, jsonStr);
            }
            return JsonMapper.ToObject<List<BaseCom>>(jsonStr);
        }

        public Entity CreateEntity(string pUid,int pEntityId)
        {
            List<BaseCom> resComs = GetEntityComsModel(pEntityId);
            Entity entity = new Entity(pUid);

            //组件
            for (int i = 0; i < resComs.Count; i++)
            {
                entity.AddCom(resComs[i]);
            }

            //保存
            AddEntity(entity);

            return entity;
        }

        public void AddEntity(Entity entity)
        {
            if (entityDict.ContainsKey(entity.Uid))
            {
                ECSLocate.Log.LogError("保存实体失败，重复的Uid", entity.Uid);
                return;
            }

            entityDict.Add(entity.Uid, entity);

            //实体初始化
            entity.Init();

            //创建实体数据流
            EntityWorkData entityWorkData = new EntityWorkData(entity.Uid, entity);
            entityWorkData.Uid = entity.Uid;
            ECSLayerLocate.Info.AddEntityWorkData(entity.Uid, entityWorkData);

            //组件Awake
            foreach (var item in entity.GetComs())
            {
                item.Awake(entity);
            }
        }

        public Entity GetEntity(string uid)
        {
            return entityDict[uid];
        }

        public List<Entity> GetEntitys(int pEntityId)
        {
            Dictionary<string, Entity> entityMap = GetAllEntitys();
            List<Entity> entities = new List<Entity>();
            foreach (Entity entity in entityMap.Values)
            {
                if (entity.EntityId == pEntityId)
                {
                    entities.Add(entity);
                }
            }

            return entities;
        }

        public Dictionary<string, Entity> GetAllEntitys()
        {
            return entityDict;
        }

        public void CheckEntityInSystem(string uid)
        {
            Entity entity = GetEntity(uid);
            if (entity == null)
                return;
            CheckEntityInSystem(entity);
        }

        #endregion

        #region System

        public void RegSystem(BaseSystem system)
        {
            if (systemList.Contains(system))
                return;
            systemList.Add(system);
        }

        public void ExcuteUpdateSystem()
        {
            for (int i = 0; i < systemList.Count; i++)
            {
                systemList[i].Excute();
            }
        }

        public void ExcuteFixedUpdateSystem()
        {
            for (int i = 0; i < systemList.Count; i++)
            {
                systemList[i].FixedUpdateExecute();
            }
        }
        
        #endregion

        public void Clear()
        {
            entityCnf.Clear();
        }
    }
}
