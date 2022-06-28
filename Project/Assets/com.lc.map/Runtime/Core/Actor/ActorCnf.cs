using LCConfig;
using System.Collections;
using UnityEngine;
using LCToolkit;
using System.Collections.Generic;
using LCECS.Core;

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

        [ConfigValue("默认交互")]
        public List<ActorInteractive> interactions = new List<ActorInteractive>();

        [ConfigValue("交互范围")]
        public float interactiveRange = 1;

        [ConfigValue("组件配置")]
        public List<BaseCom> comCnfs = new List<BaseCom>();

        public IConfig Clone()
        {
            ActorCnf cnf = new ActorCnf();
            cnf.id = id++;
            cnf.type = type;
            cnf.name = name;
            cnf.entityId = entityId;
            cnf.prefab = prefab;
            cnf.interactions = interactions;
            cnf.comCnfs = comCnfs;
            return cnf;
        }
    }
}