using System.Collections.Generic;
using System;
using System.Collections.Generic;
using Config;

namespace Demo.Config
{
    
    public class TbItemRepairCnf : Dictionary<int, ItemRepairCnf>
    {
        
        public void AddConfig(int key1, ItemRepairCnf config)
        {
            
            if (!this.ContainsKey(key1))
            {
                this.Add(key1, config);
            }
        }

        public void AddConfig(List<ItemRepairCnf> configs)
        {
            foreach (var item in configs)
            {
                ItemRepairCnf config = item;
                AddConfig(config.id, config);
            }
        }

    }

}

