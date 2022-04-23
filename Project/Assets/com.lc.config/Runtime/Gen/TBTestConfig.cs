using LCJson;
using LCLoad;
using System.Collections;
using System.Collections.Generic;
using System;


namespace LCConfig
{
    /// <summary>
    /// 测试配置
    /// </summary>
    public class TBTestConfig : Dictionary<Int32, Dictionary<String, Dictionary<String, TestConfig>>>
    {
        /// <summary>
        /// 添加测试
        /// </summary>
		/// <param name="key1">ccc</param>
		/// <param name="key2">aa</param>
		/// <param name="key3">bbbb</param>
        public void AddConfig(Int32 key1, String key2, String key3, TestConfig config)
        {
            
            if (!this.ContainsKey(key1))
            {
                this.Add(key1, new Dictionary<String, Dictionary<String, TestConfig>>());
            }
            if (!this[key1].ContainsKey(key2))
            {
                this[key1].Add(key2, new Dictionary<String, TestConfig>());
            }
            if (!this[key1][key2].ContainsKey(key3))
            {
                this[key1][key2].Add(key3, config);
            }
        }

        /// <summary>
        /// 添加测试
        /// </summary>
        public void AddConfig(List<IConfig> configs)
        {
            foreach (var item in configs)
            {
                TestConfig config = (TestConfig)item;
                AddConfig(config.key, config.name, config.aaaaa, config);
            }
        }

    }
}

