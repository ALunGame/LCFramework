using LCJson;
using LCLoad;
using System.Collections;
using System.Collections.Generic;
using LCMap;
using System;
using LCToolkit;
using System.Collections.Generic;


namespace LCConfig
{
    /// <summary>
    /// ActorCnf配置
    /// </summary>
    public class TBActorCnf : Dictionary<Int32, ActorCnf>
    {
        /// <summary>
        /// 添加ActorCnf
        /// </summary>
		/// <param name="key1">演员Id</param>
        public void AddConfig(Int32 key1, ActorCnf config)
        {
            
            if (!this.ContainsKey(key1))
            {
                this.Add(key1, config);
            }
        }

        /// <summary>
        /// 添加ActorCnf
        /// </summary>
        public void AddConfig(List<ActorCnf> configs)
        {
            foreach (var item in configs)
            {
                ActorCnf config = item;
                AddConfig(config.id, config);
            }
        }

    }
}

