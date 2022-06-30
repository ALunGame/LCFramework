using LCConfig;
using LCToolkit;
using System.Collections.Generic;

namespace Demo.Config
{
    public class ProduceCnf : IConfig
    {
        [ConfigKey(1, "生产Id")]
        public int id;

        [ConfigValue("生产时间")]
        public float time;

        [ConfigValue("生产物品")]
        public List<BagItem> produceItems = new List<BagItem>();

        [ConfigValue("消耗物品")]
        public List<BagItem> costItems = new List<BagItem>();

        public IConfig Clone()
        {
            ProduceCnf cnf = new ProduceCnf();
            cnf.id = id++;
            return cnf;
        }

        public bool CheckCanMake(Dictionary<int, int> itemDict)
        {
            foreach (var costItem in costItems)
            {
                if (itemDict.ContainsKey(costItem.id))
                {
                    int cnt = itemDict[costItem.id];
                    if (cnt < costItem.cnt)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}