using LCConfig;
using LCToolkit;

namespace Demo.Config
{
    /// <summary>
    /// 物品配置
    /// </summary>
    public class ItemCnf : IConfig
    {
        [ConfigKey(1, "物品Id")]
        public int id;

        [ConfigValue("物品名")]
        public string name = "";

        [ConfigValue("物品图")]
        public UnityObjectAsset sprite = new UnityObjectAsset(UnityObjectAsset.AssetType.Sprite);


        public IConfig Clone()
        {
            ItemCnf cnf = new ItemCnf();
            return cnf;
        }
    }
}
