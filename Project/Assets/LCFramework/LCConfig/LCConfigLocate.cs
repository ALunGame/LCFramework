using System.Collections.Generic;

namespace LCConfig
{
    /// <summary>
    /// 自己随便写的玩意(别参考)
    /// </summary>
    public static class LCConfigLocate
    {
        public const string SettingJsonPath = "./Assets/Demo/Setting/ConfigSetting";
        public static bool ReLoadData = false;

        private static LCConfigServer configServer;

        public static void Init()
        {
            configServer = new LCConfigServer();
        }

        public static Config GetConfig(string name)
        {
            return configServer.GetConfig(name);
        }

        public static Dictionary<string, string> GetConfigItemDataDict(string cnfName, string key, string itemName = "Id")
        {
            return configServer.GetConfigItemDataDict(cnfName, key, itemName);
        }

        public static string GetConfigItemValue(string cnfName, string key, string returnItemName, string keyItemName = "Id")
        {
            return configServer.GetConfigItemValue(cnfName, key, returnItemName,keyItemName);
        }
    }
}
