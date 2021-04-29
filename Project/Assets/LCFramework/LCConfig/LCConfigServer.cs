using LCHelp;
using System.Collections.Generic;
using XPToolchains.Json;

namespace LCConfig
{
    public class LCConfigServer
    {
        private ConfigSetting settingJson = null;
        private Dictionary<string, Config> cacheConfigDict = null;

        private void LoadConfig()
        {
            if (LCConfigLocate.ReLoadData)
            {
                settingJson = null;
                cacheConfigDict = null;
                LCConfigLocate.ReLoadData = false;
            }

            if (settingJson == null)
            {
                string dataJson = LCIO.ReadText(LCConfigLocate.SettingJsonPath);
                settingJson = JsonMapper.ToObject<ConfigSetting>(dataJson);
            }

            if (cacheConfigDict == null)
            {
                cacheConfigDict = new Dictionary<string, Config>();
                List<string> files = LCIO.GetAllFilePath(settingJson.JsonPath);
                for (int i = 0; i < files.Count; i++)
                {
                    string dataJson = LCIO.ReadText(files[i]);
                    ConfigGroup cfgGroup = JsonMapper.ToObject<ConfigGroup>(dataJson);
                    for (int j = 0; j < cfgGroup.Configs.Count; j++)
                    {
                        cacheConfigDict.Add(cfgGroup.Configs[j].Name, cfgGroup.Configs[j]);
                    }
                }
            }
        }

        /// <summary>
        /// 获得配置
        /// </summary>
        /// <param name="cnfName">配置名</param>
        /// <returns></returns>
        public Config GetConfig(string cnfName)
        {
            LoadConfig();
            if (!cacheConfigDict.ContainsKey(cnfName))
            {
                LCLog.LogError("没有指定的配置》》》》", cnfName);
                return null;
            }
            return cacheConfigDict[cnfName];
        }

        /// <summary>
        /// 获得指定配置的指定配置项
        /// </summary>
        /// <param name="cnfName">配置名</param>
        /// <param name="key">指定值</param>
        /// <param name="itemName">配置项名（默认Id）</param>
        /// <returns></returns>
        public Dictionary<string, string> GetConfigItemDataDict(string cnfName, string key, string itemName = "Id")
        {
            Config config = GetConfig(cnfName);
            if (config == null)
            {
                return null;
            }

            int searchDataIndex = -1;
            for (int i = 0; i < config.Items.Count; i++)
            {
                ConfigItem configItem = config.Items[i];
                if (configItem.Name == itemName)
                {
                    for (int j = 0; j < configItem.DataList.Count; j++)
                    {
                        if (configItem.DataList[j] == key)
                        {
                            searchDataIndex = j;
                            break;
                        }
                    }
                }
                if (searchDataIndex >= 0)
                {
                    break;
                }
            }

            if (searchDataIndex < 0)
            {
                LCLog.LogError("GetConfigItemDataDict 找到配置项 >>>>>", cnfName, key, itemName);
                return null;
            }

            Dictionary<string, string> dataDict = new Dictionary<string, string>();
            for (int i = 0; i < config.Items.Count; i++)
            {
                dataDict.Add(config.Items[i].Name, config.Items[i].DataList[searchDataIndex]);
            }
            return dataDict;
        }

        public string GetConfigItemValue(string cnfName, string key, string returnItemName,string keyItemName = "Id")
        {
            Dictionary<string, string> attrDict = GetConfigItemDataDict(cnfName, key, keyItemName);
            if (attrDict==null || attrDict.Count<=0)
            {
                LCLog.LogError("GetConfigItemValue 没有配置项 >>>>>", cnfName, key, keyItemName);
                return null;
            }
            foreach (var item in attrDict)
            {
                if (item.Key == returnItemName)
                {
                    return item.Value;
                }
            }
            LCLog.LogError("GetConfigItemValue没有指定 配置项 >>>>>", cnfName, key, keyItemName,returnItemName);
            return null;
        }
    }
}
