using System.Collections.Generic;
using System;
using System.Collections.Generic;
using Cnf;
using Cnf;

namespace Cnf
{
    
    public class TbActorCollectCnf : Dictionary<int, ActorCollectCnf>
    {
        
        public void AddConfig(int key1, ActorCollectCnf config)
        {
            
            if (!this.ContainsKey(key1))
            {
                this.Add(key1, config);
            }
        }

        public void AddConfig(List<ActorCollectCnf> configs)
        {
            foreach (var item in configs)
            {
                ActorCollectCnf config = item;
                AddConfig(config.id, config);
            }
        }

    }

}

