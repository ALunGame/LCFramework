using System.Collections.Generic;
using Config;

namespace Demo.UserData
{
    /// <summary>
    /// 村庄数据
    /// </summary>
    public class VillageData : BaseUserData<VillageData>
    {
        private Dictionary<ItemType, List<ItemInfo>> itemMap = new Dictionary<ItemType, List<ItemInfo>>();

        public Dictionary<ItemType, List<ItemInfo>> ItemMap
        {
            get
            {
                return itemMap;
            }
        }
        
        
        
    }
}