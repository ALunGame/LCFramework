using System.Collections.Generic;
using System;

namespace Cnf
{
    
    public class TbActorLifeCnf : Dictionary<int, ActorLifeCnf>
    {
        
        public void AddConfig(int key1, ActorLifeCnf config)
        {
            
            if (!this.ContainsKey(key1))
            {
                this.Add(key1, config);
            }
        }

        public void AddConfig(List<ActorLifeCnf> configs)
        {
            foreach (var item in configs)
            {
                ActorLifeCnf config = item;
                AddConfig(config.id, config);
            }
        }

    }

}

