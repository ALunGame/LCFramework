using LCHelp;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XPToolchains.Json;

namespace LCConfig
{
    /// <summary>
    /// 配置辅助接口
    /// </summary>
    public class LCConfigHelp
    {
        //配置数据起始位置
        public static int ConfigItemValueIndex = 5;

        #region Check

        //检测
        public static bool CheckRelateDataIsLegal(ConfigJson configJson, ConfigItem item, string setData)
        {
            if (item.RuleType != ConfigDataRuleType.Relate)
            {
                return false;
            }

            string dataValue = item.RuleData;
            string[] confList = dataValue.Split('/');
            if (confList == null || confList.Length < 3)
            {
                Debug.LogError("引用的数据类型没有指定对应的配置");
                return false;
            }

            string groupName = confList[0];
            string configName = confList[1];
            string itemName = confList[2];

            if (!configJson.ConfGroup.ContainsKey(groupName))
            {
                Debug.LogError("没有此配置组" + groupName);
                return false;
            }

            ConfigGroup group = configJson.ConfGroup[groupName];
            Config config = GetConfig(group, configName);
            if (config == null)
            {
                Debug.LogError("没有此配置" + configName);
                return false;
            }

            List<string> dataList = GetConfigItemDataList(config, itemName);
            if (dataList == null || dataList.Count <= 0)
            {
                Debug.LogError("没有此配置数据" + configName);
                return false;
            }
            for (int i = 0; i < dataList.Count; i++)
            {
                if (dataList[i] == setData)
                {
                    return true;
                }
            }
            Debug.LogError(string.Format("此配置数据非法！！！！！关系配置中没有该字段 {0}/{1}/{2}  {3}", groupName, configName, itemName, setData));
            return false;
        }

        //检测重复
        public static bool CheckNoSameDataIsLegal(ConfigItem item, string setData)
        {
            if (item.RuleType != ConfigDataRuleType.NoSame && setData != SetConfigItemDefaultValue(item))
            {
                return true;
            }
            if (item.DataList.Contains(setData))
            {
                return false;
            }
            return true;
        }

        #endregion

        #region Get

        //获得配置
        public static Config GetConfig(ConfigGroup configGroup, string name)
        {
            if (configGroup == null)
            {
                return null;
            }
            for (int i = 0; i < configGroup.Configs.Count; i++)
            {
                if (configGroup.Configs[i].Name == name)
                {
                    return configGroup.Configs[i];
                }
            }
            return null;
        }

        //获得配置项
        public static ConfigItem GetConfigItem(Config selConf, string name)
        {
            for (int i = 0; i < selConf.Items.Count; i++)
            {
                if (selConf.Items[i].Name == name)
                {
                    return selConf.Items[i];
                }
            }
            return null;
        }

        //获得所有的配置路径名
        public static Dictionary<string, List<string>> GetAllConfigPathName(ConfigJson configJson)
        {
            Dictionary<string, List<string>> resDict = new Dictionary<string, List<string>>();

            foreach (var item in configJson.ConfGroup.Values)
            {
                List<string> resList = new List<string>();
                for (int i = 0; i < item.Configs.Count; i++)
                {
                    resList.Add(item.Configs[i].Name);
                }

                resDict.Add(item.Name, resList);
            }

            return resDict;
        }

        //获得所有的配置项路径
        public static List<string> GetAllConfigItemPathName(ConfigJson configJson)
        {
            List<string> resList = new List<string>();

            foreach (var item in configJson.ConfGroup.Values)
            {
                ConfigGroup confGroup = item;
                for (int i = 0; i < confGroup.Configs.Count; i++)
                {
                    Config config = confGroup.Configs[i];

                    for (int j = 0; j < config.Items.Count; j++)
                    {
                        ConfigItem confItem = config.Items[j];
                        resList.Add(string.Format("{0}/{1}/{2}", confGroup.Name, config.Name, confItem.Name));
                    }
                }

            }

            return resList;
        }

        //获得配置项数据数量
        public static int GetConfigItemDataCnt(Config conf)
        {
            if (conf.Items.Count <= 0)
            {
                return 0;
            }
            return conf.Items[0].DataList.Count;
        }

        //获得配置项数据
        public static List<string> GetConfigItemDataList(Config conf, string itemName)
        {
            if (conf.Items.Count <= 0)
            {
                return null;
            }
            for (int i = 0; i < conf.Items.Count; i++)
            {
                if (conf.Items[i].Name == itemName)
                {
                    return conf.Items[i].DataList;
                }
            }
            return null;
        }

        public static List<ConfigItem> GetAllRelateConfigItems(ConfigJson configJson)
        {
            List<ConfigItem> configItems = new List<ConfigItem>();

            foreach (var item in configJson.ConfGroup.Values)
            {
                for (int i = 0; i < item.Configs.Count; i++)
                {
                    Config config = item.Configs[i];
                    for (int j = 0; j < config.Items.Count; j++)
                    {
                        if (config.Items[j].RuleType == ConfigDataRuleType.Relate)
                        {
                            configItems.Add(config.Items[j]);
                        }
                    }
                }
            }

            return configItems;
        }

        public static List<SearchShowConfigGroup> GetSearchData(ConfigJson configJson, string data, ConfigDataType dataType)
        {
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }
            List<SearchShowConfigGroup> resList = new List<SearchShowConfigGroup>();

            foreach (var item in configJson.ConfGroup.Values)
            {
                SearchShowConfigGroup searchGroup = null;
                for (int i = 0; i < item.Configs.Count; i++)
                {
                    Config config = item.Configs[i];
                    for (int j = 0; j < config.Items.Count; j++)
                    {
                        ConfigItem configItem = config.Items[j];
                        if (configItem.Type == dataType)
                        {
                            //找到了
                            for (int z = 0; z < configItem.DataList.Count; z++)
                            {
                                if (configItem.DataList[i] == data)
                                {
                                    if (searchGroup == null)
                                    {
                                        searchGroup = new SearchShowConfigGroup(item.Name);
                                        resList.Add(searchGroup);
                                    }
                                    if (!searchGroup.ConfigDataDict.ContainsKey(config.Name))
                                    {
                                        searchGroup.ConfigDataDict.Add(config.Name, new Dictionary<string, List<int>>());
                                    }
                                    if (!searchGroup.ConfigDataDict[config.Name].ContainsKey(configItem.Name))
                                    {
                                        searchGroup.ConfigDataDict[config.Name].Add(configItem.Name, new List<int>());
                                    }
                                    searchGroup.ConfigDataDict[config.Name][configItem.Name].Add(z);
                                }
                            }
                        }

                    }
                }
            }

            return resList;
        }

        #endregion

        #region Add

        //添加配置
        public static void AddConfig(ConfigJson configJson, ConfigGroup configGroup, string name)
        {
            Dictionary<string, List<string>> resDict = GetAllConfigPathName(configJson);
            foreach (string key in resDict.Keys)
            {
                List<string> item = resDict[key];
                if (item.Contains(name))
                {
                    Debug.LogError(string.Format("添加配置失败，该配置名已经在 {0} - {1} 中", key, name));
                    return;
                }
            }

            Config config = new Config(name);
            configGroup.Configs.Add(config);

            //默认带一个Id
            ConfigItem idItem = new ConfigItem("Id");
            idItem.Type = ConfigDataType.Int;
            idItem.RuleType = ConfigDataRuleType.NoSame;

            config.Items.Add(idItem);
        }

        //添加配置项
        public static void AddConfigItem(Config conf, string name)
        {
            for (int i = 0; i < conf.Items.Count; i++)
            {
                if (conf.Items[i].Name == name)
                {
                    Debug.LogError("配置名重复");
                    return;
                }
            }

            int dataCnt = GetConfigItemDataCnt(conf);
            ConfigItem configItem = new ConfigItem(name);
            for (int i = 0; i < dataCnt; i++)
            {
                configItem.DataList.Add("");
            }
            conf.Items.Add(configItem);

        }

        #endregion

        #region Set

        public static string SetConfigItemDefaultValue(ConfigItem item)
        {
            string defaultValue;
            switch (item.Type)
            {
                case ConfigDataType.Int:
                    defaultValue = "0";
                    break;
                case ConfigDataType.Float:
                    defaultValue = "0";
                    break;
                case ConfigDataType.String:
                    defaultValue = "";
                    break;
                case ConfigDataType.Vector2:
                    defaultValue = "(0,0)";
                    break;
                case ConfigDataType.Vector3:
                    defaultValue = "(0,0,0)";
                    break;
                case ConfigDataType.Vector4:
                    defaultValue = "(0,0,0,0)";
                    break;
                case ConfigDataType.Bool:
                    defaultValue = "false";
                    break;
                default:
                    defaultValue = "";
                    break;
            }
            return defaultValue;
        }

        public static string SetConfigItemValue(ConfigJson configJson, ConfigItem item, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "";
            }
            if (item.RuleType == ConfigDataRuleType.Relate)
            {
                if (CheckRelateDataIsLegal(configJson, item, value))
                {
                    return value;
                }
                return SetConfigItemDefaultValue(item);
            }
            else if (item.RuleType == ConfigDataRuleType.NoSame)
            {
                if (CheckNoSameDataIsLegal(item, value))
                {
                    return value;
                }
                return SetConfigItemDefaultValue(item);
            }
            return value;
        }

        #endregion

        #region Load

        public static ConfigGroup LoadConfigGroup(string name)
        {
            string settingPath = LCConfigLocate.SettingJsonPath;
            if (!File.Exists(settingPath))
                return null;

            string dataJson = EDTool.ReadText(settingPath);
            ConfigSetting configSetting = JsonMapper.ToObject<ConfigSetting>(dataJson);
            if (configSetting == null)
                return null;

            dataJson = EDTool.ReadText(configSetting.JsonPath + "/" + name);
            ConfigGroup configGroup = JsonMapper.ToObject<ConfigGroup>(dataJson);
            if (configGroup == null)
                return null;

            return configGroup;
        }

        #endregion

    }
}
