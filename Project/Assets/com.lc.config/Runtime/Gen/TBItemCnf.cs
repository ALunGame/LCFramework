using System.Collections.Generic;
using System;

namespace Demo.Config
{
    
    public class TbItemCnf : Dictionary<int, ItemCnf>
    {
        
        public void AddConfig(int key1, ItemCnf config)
        {
            
            if (!this.ContainsKey(key1))
            {
                this.Add(key1, config);
            }
        }

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

