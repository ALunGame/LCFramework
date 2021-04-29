using System;
using System.Collections.Generic;

namespace LCConfig
{
    /// <summary>
    /// 配置设置
    /// </summary>
    [Serializable]
    public class ConfigSetting
    {
        public string ExcelRootPath = "";
        public string LuaRootPath = "";
        public string JsonPath = "";

        //自动生成Lua配置文件
        public bool AutoCreateLuaConfig = true;

        //自动生成Json文件
        public bool AutoCreateJson = true;

        public ConfigSetting()
        {
            ExcelRootPath = "./Excels";
            LuaRootPath = "./Assets/GameAssets/Lua/Config";
            JsonPath = "./Assets/LCConfig/Editor/ConfigJsons";
        }
    }


    [Serializable]
    public class ConfigJson
    {
        public Dictionary<string, ConfigGroup> ConfGroup = new Dictionary<string, ConfigGroup>();
    }

    [Serializable]
    public class ConfigGroup
    {
        public string Name = "";
        public List<Config> Configs = new List<Config>();
        public ConfigGroup()
        {

        }
        public ConfigGroup(string name)
        {
            Name = name;
        }
    }

    [Serializable]
    public class Config
    {
        public string Name = "";
        public List<ConfigItem> Items = new List<ConfigItem>();
        public Config()
        {

        }
        public Config(string name)
        {
            Name = name;
        }
    }

    /// <summary>
    /// 配置数据类型
    /// </summary>
    public enum ConfigDataType
    {
        Int,
        Float,
        String,
        Vector2,
        Vector3,
        Vector4,
        Bool,               
        Asset,              //资源
    }

    public enum ConfigDataRuleType
    {
        None,              //没有规则
        NoSame,            //去重
        Relate,            //与其他模板关联
    }

    [Serializable]
    public class ConfigItem
    {
        //第一行
        public string Name = "";
        //第二行
        public ConfigDataType Type = ConfigDataType.Int;
        //第三行
        public ConfigDataRuleType RuleType = ConfigDataRuleType.None;
        //第四行
        public string RuleData = "";
        //第五行
        public string Desc = "注释";

        //分割数据
        public List<string> DataList = new List<string>();

        public ConfigItem()
        {

        }

        public ConfigItem(string name)
        {
            Name = name;
        }
    }
}
