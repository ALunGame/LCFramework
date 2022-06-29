using LCConfig;
using LCToolkit;
using System.Collections.Generic;

namespace Demo.Config
{
    public class ProduceCnf : IConfig
    {
        [ConfigKey(1, "生产Id")]
        public int id;

        [ConfigKey(1, "生产时间")]
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
    }
}