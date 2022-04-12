using LCECS.Data;
using LCECS.Layer.Info;
using LCHelp;
using LCJson;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LCECS.Server.Layer
{
    public class InfoServer : IInfoServer
    {
        private Dictionary<int, ISensor> SensorDict = new Dictionary<int, ISensor>();
        private Dictionary<int, EntityWorkData> WorkDataDict = new Dictionary<int, EntityWorkData>();
        private EntityJsonList entityJsons = null;

        private void RegAllSensor()
        {
            List<Type> sensorTypes = LCReflect.GetInterfaceByType<ISensor>();
            if (sensorTypes == null)
                return;

            //世界信息
            foreach (Type type in sensorTypes)
            {
                WorldSensorAttribute attr = LCReflect.GetTypeAttr<WorldSensorAttribute>(type);
                if (attr == null)
                {
                    ECSLocate.Log.LogR("有世界信息没有加入特性 >>>>>>", type.Name);
                    return;
                }

                ISensor sensor = LCReflect.CreateInstanceByType<ISensor>(type.FullName);
                SensorDict.Add((int)attr.InfoKey, sensor);
            }
        }
        
        private T GetSensor<T>(int key) where T : ISensor
        {
            if (!SensorDict.ContainsKey(key))
            {
                ECSLocate.Log.LogError("没有找到对应的世界信息》》》》" + key);
                return default ;
            }
            return (T)SensorDict[key];
        }
        
        public void Init()
        {
            TextAsset jsonData = ECSLocate.Factory.GetProduct<TextAsset>(FactoryType.Asset, null, ECSDefPath.EntityJsonPath);
            EntityJsonList json = JsonMapper.ToObject<EntityJsonList>(jsonData.text);
            this.entityJsons = json;
            RegAllSensor();
        }

        public T GetSensor<T>(SensorType key) where T : ISensor
        {
            return GetSensor<T>((int) key);
        }

        public EntityJson GetEntityConf(int entityId)
        {
            if (entityJsons == null)
                return null;
            for (int i = 0; i < entityJsons.List.Count; i++)
            {
                EntityJson json = entityJsons.List[i];
                if (json.EntityId == entityId)
                {
                    return json;
                }
            }
            return null;
        }
        
        public void AddEntityWorkData(int entityId, EntityWorkData data)
        {
            if (WorkDataDict.ContainsKey(entityId))
            {
                return;
            }
            WorkDataDict.Add(entityId, data);
        }

        public EntityWorkData GetEntityWorkData(int entityId)
        {
            if (WorkDataDict.ContainsKey(entityId))
            {
                return WorkDataDict[entityId];
            }
            return null;
        }

        public void RemoveEntityWorkData(int entityId)
        {
            if (WorkDataDict.ContainsKey(entityId))
            {
                WorkDataDict.Remove(entityId);
            }
        }
    }
}
