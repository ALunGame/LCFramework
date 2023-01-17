using System.Collections.Generic;
using System;
using LCMap;
using System.Collections.Generic;
using System;

namespace LCMap
{
    
    public class TbActorCnf : Dictionary<int, ActorCnf>
    {
        
        public void AddConfig(int key1, ActorCnf config)
        {
            
            if (!this.ContainsKey(key1))
            {
                this.Add(key1, config);
            }
        }

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

