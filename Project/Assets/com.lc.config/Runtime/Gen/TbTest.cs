using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System;
using System;
using Cnf;
using System.Collections.Generic;
using Cnf;
using System.Collections.Generic;
using Cnf;
using Cnf;

namespace TT
{
    
    public class TbTest : Dictionary<int, Dictionary<int, Dictionary<string, Test>>>
    {
        
        public void AddConfig(int key1, int key2, string key3, Test config)
        {
            
            if (!this.ContainsKey(key1))
            {
                this.Add(key1, new Dictionary<int, Dictionary<string, Test>>());
            }
            if (!this[key1].ContainsKey(key2))
            {
                this[key1].Add(key2, new Dictionary<string, Test>());
            }
            if (!this[key1][key2].ContainsKey(key3))
            {
                this[key1][key2].Add(key3, config);
            }
        }

        public void AddConfig(List<Test> configs)
        {
            foreach (var item in configs)
            {
                Test config = item;
                AddConfig(config.id, config.id2, config.name, config);
            }
        }

    }

}

