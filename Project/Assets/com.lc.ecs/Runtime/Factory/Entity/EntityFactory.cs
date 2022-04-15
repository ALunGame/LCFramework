using LCECS.Core;
using LCJson;
using LCToolkit;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LCECS.Server.Factory
{
    public class EntityFactory : IFactory<Entity>
    {
        private Dictionary<int, Entity> EntityModelDict = new Dictionary<int, Entity>();

        public Entity CreateProduct(Action<object[]> func, params object[] data)
        {
            //解析参数
            int entityId        = int.Parse(data[0].ToString());
            int entityConfId    = int.Parse(data[1].ToString());
            GameObject entityGo = data.Length > 2 ? data[2] as GameObject : null;

            //配置数据
            Entity entityModel = GetEntityModel(entityConfId);
            if (entityModel == null)
            {
                ECSLocate.Log.LogError("实体配置数据不存在>>>>>>>", entityConfId);
                return null;
            }

            //创建实体
            Entity entity = DeepCopy<Entity, Entity>.Trans(entityModel);
            entity.SetEntityGo(entityGo);
            entity.Init(entityId);
            foreach (BaseCom com in entity.GetComs())
            {
                com.Init(entity);
            }
            func?.Invoke(new object[] { entityGo });

            ECSLocate.Log.LogR("创建实体成功>>>>>>>>", entityConfId);
            return entity;
        }

        /// <summary>
        /// 获得实体配置
        /// </summary>
        /// <returns></returns>
        private Entity GetEntityModel(int id)
        {
            if (!EntityModelDict.ContainsKey(id))
            {
                Entity entity = LoadEntityModel(id);
                if (entity != null)
                {
                    EntityModelDict.Add(id, entity);
                }
            }
            return EntityModelDict[id];
        }

        /// <summary>
        /// 加载实体配置
        /// </summary>
        /// <returns></returns>
        private Entity LoadEntityModel(int id)
        {
            TextAsset jsonData = ECSLocate.Factory.GetProduct<TextAsset>(FactoryType.Asset, null, ECSDefPath.GetEntityPath(id.ToString()));
            if (jsonData == null)
                return null;
            Entity entity = JsonMapper.ToObject<Entity>(jsonData.text);
            if (entity == null)
                return null;
            return entity;
        }
    }
}
