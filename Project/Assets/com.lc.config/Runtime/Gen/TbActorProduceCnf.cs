using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System;
using Cnf;

namespace Cnf
{
    
    public class TbActorProduceCnf : Dictionary<int, ActorProduceCnf>
    {
        
        public void AddConfig(int key1, ActorProduceCnf config)
        {
            
            if (!this.ContainsKey(key1))
            {
                this.Add(key1, config);
            }
        }

        public void AddConfig(List<ActorProduceCnf> configs)
        {
            foreach (var item in configs)
            {
                ActorProduceCnf config = item;
                AddConfig(config.id, config);
            }
        }

    }

}

