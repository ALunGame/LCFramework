using LCConfig;
using System.Collections;
using UnityEngine;
using LCToolkit;

namespace LCMap
{
    /// <summary>
    /// 演员配置
    /// </summary>
    public class ActorCnf : IConfig
    {
        [ConfigKey(1,"演员Id")]
        public int id = 0;

        [ConfigValue("演员类型")]
        public ActorType type = ActorType.Villager;

        [ConfigValue("演员名")]
        public string name = "";

        [ConfigValue("演员实体Id")]
        public int entityId = 0;

        [ConfigValue("演员预制体")]
        public UnityObjectAsset prefab = new UnityObjectAsset();

        public IConfig Clone()
        {
            ActorCnf cnf = new ActorCnf();
            return cnf;
        }
    }
}