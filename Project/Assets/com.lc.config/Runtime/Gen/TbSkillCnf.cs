using System.Collections.Generic;
using System;

namespace Demo.Config
{
    
    public class TbSkillCnf : Dictionary<int, SkillCnf>
    {
        
        public void AddConfig(int key1, SkillCnf config)
        {
            
            if (!this.ContainsKey(key1))
            {
                this.Add(key1, config);
            }
        }

        public void AddConfig(List<SkillCnf> configs)
        {
            foreach (var item in configs)
            {
                SkillCnf config = item;
                AddConfig(config.id, config);
            }
        }

    }

}

