using System.Collections.Generic;

using System;
using LCUI;

namespace Demo
{
    
    public class TbUIPanelCnf : Dictionary<UIPanelDef, UIPanelCnf>
    {
        
        public void AddConfig(UIPanelDef key1, UIPanelCnf config)
        {
            
            if (!this.ContainsKey(key1))
            {
                this.Add(key1, config);
            }
        }

        public void AddConfig(List<UIPanelCnf> configs)
        {
            foreach (var item in configs)
            {
                UIPanelCnf config = item;
                AddConfig(config.id, config);
            }
        }

    }

}

