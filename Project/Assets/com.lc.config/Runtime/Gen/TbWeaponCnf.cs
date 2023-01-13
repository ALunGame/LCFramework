using System.Collections.Generic;
using System;

namespace Demo.Config
{
    
    public class TbWeaponCnf : Dictionary<int, WeaponCnf>
    {
        
        public void AddConfig(int key1, WeaponCnf config)
        {
            
            if (!this.ContainsKey(key1))
            {
                this.Add(key1, config);
            }
        }

        public void AddConfig(List<WeaponCnf> configs)
        {
            foreach (var item in configs)
            {
                WeaponCnf config = item;
                AddConfig(config.id, config);
            }
        }

    }

}

