using System.Collections.Generic;
using System;

namespace Demo.Config
{
    
    public class TbActorBasePropertyCnf : Dictionary<int, ActorBasePropertyCnf>
    {
        
        public void AddConfig(int key1, ActorBasePropertyCnf config)
        {
            
            if (!this.ContainsKey(key1))
            {
                this.Add(key1, config);
            }
        }

        public void AddConfig(List<ActorBasePropertyCnf> configs)
        {
            foreach (var item in configs)
            {
                ActorBasePropertyCnf config = item;
                AddConfig(config.id, config);
            }
        }

    }

}

