using System.Collections.Generic;
using System;
using System.Collections.Generic;
using Cnf;

namespace Cnf
{
    
    public class TbItemRecipeCnf : Dictionary<int, ItemRecipeCnf>
    {
        
        public void AddConfig(int key1, ItemRecipeCnf config)
        {
            
            if (!this.ContainsKey(key1))
            {
                this.Add(key1, config);
            }
        }

        public void AddConfig(List<ItemRecipeCnf> configs)
        {
            foreach (var item in configs)
            {
                ItemRecipeCnf config = item;
                AddConfig(config.id, config);
            }
        }

    }

}

