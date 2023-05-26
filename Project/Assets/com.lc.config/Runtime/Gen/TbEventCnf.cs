using System.Collections.Generic;
using System;
using Cnf;
using System.Collections.Generic;
using Cnf;

namespace Cnf
{
    
    public class TbEventCnf : Dictionary<int, EventCnf>
    {
        
        public void AddConfig(int key1, EventCnf config)
        {
            
            if (!this.ContainsKey(key1))
            {
                this.Add(key1, config);
            }
        }

        public void AddConfig(List<EventCnf> configs)
        {
            foreach (var item in configs)
            {
                EventCnf config = item;
                AddConfig(config.id, config);
            }
        }

    }

}

