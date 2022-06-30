using LCJson;
using LCLoad;
using System.Collections;
using System.Collections.Generic;
using Demo.Config;
using System;
using System.Collections.Generic;


namespace LCConfig
{
    /// <summary>
    /// ProduceCnf配置
    /// </summary>
    public class TBProduceCnf : Dictionary<Int32, ProduceCnf>
    {
        /// <summary>
        /// 添加ProduceCnf
        /// </summary>
		/// <param name="key1">生产Id</param>
        public void AddConfig(Int32 key1, ProduceCnf config)
        {
            
            if (!this.ContainsKey(key1))
            {
                this.Add(key1, config);
            }
        }

        /// <summary>
        /// 添加ProduceCnf
        /// </summary>
        public void AddConfig(List<ProduceCnf> configs)
        {
            foreach (var item in configs)
            {
                ProduceCnf config = item;
                AddConfig(config.id, config);
            }
        }

    }
}

