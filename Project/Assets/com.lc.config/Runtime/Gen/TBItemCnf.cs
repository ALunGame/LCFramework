using LCJson;
using LCLoad;
using System.Collections;
using System.Collections.Generic;
using Demo.Config;
using System;
using LCToolkit;


namespace LCConfig
{
    /// <summary>
    /// ItemCnf配置
    /// </summary>
    public class TBItemCnf : Dictionary<Int32, ItemCnf>
    {
        /// <summary>
        /// 添加ItemCnf
        /// </summary>
		/// <param name="key1">物品Id</param>
        public void AddConfig(Int32 key1, ItemCnf config)
        {
            
            if (!this.ContainsKey(key1))
            {
                this.Add(key1, config);
            }
        }

        /// <summary>
        /// 添加ItemCnf
        /// </summary>
        public void AddConfig(List<ItemCnf> configs)
        {
            foreach (var item in configs)
            {
                ItemCnf config = item;
                AddConfig(config.id, config);
            }
        }

    }
}

