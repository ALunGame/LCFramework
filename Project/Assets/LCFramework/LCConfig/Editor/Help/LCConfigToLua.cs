using LCHelp;
using System.IO;
using System.Text;

namespace LCConfig
{
    /// <summary>
    /// 配置转Lua
    /// </summary>
    public class LCConfigToLua
    {
        public static string LuaRootPath = "";

        private const string KeyFor = "[{0}]";
        private const string KeyValueFor = "{0}={1}";
        private const string StringFor = "\"{0}\"";
        private const string TableFor = "{0}{1} = {{\n{2}{3}}}";

        private static string CreateLuaFile(string name)
        {
            if (!Directory.Exists(LuaRootPath))
            {
                Directory.CreateDirectory(LuaRootPath);
            }
            string confPath = LuaRootPath + "/" + name + ".lua";
            if (!File.Exists(confPath))
            {
                File.Create(confPath).Dispose();
            }
            return confPath;
        }

        private static string FormatVectorValue(string name, string value, int layerCnt)
        {
            string valueLayerStr = "";
            for (int i = 0; i < layerCnt; i++)
            {
                valueLayerStr += "\t";
            }

            string tableLayerStr = "";
            for (int i = 0; i < layerCnt - 1; i++)
            {
                tableLayerStr += "\t";
            }

            StringBuilder tabContent = new StringBuilder("");

            StringBuilder valueContent = new StringBuilder("");
            string[] values = LCConvertUnity.ParseVectorValue(value);
            for (int i = 0; i < values.Length; i++)
            {
                valueContent.Append(valueLayerStr);
                valueContent.Append(string.Format(KeyValueFor, string.Format(KeyFor, i + 1), values[i]));
                valueContent.Append(",\n");
            }

            tabContent.Append(string.Format(TableFor, "", name, valueContent.ToString(), tableLayerStr));
            return tabContent.ToString();
        }

        //格式化一个键值对 （类似 id=10）
        private static void FormatKeyValue(StringBuilder context, ConfigItem item, int dataIndex, int layerCnt)
        {
            string layerStr = "";
            for (int i = 0; i < layerCnt; i++)
            {
                layerStr += "\t";
            }
            context.Append(layerStr);
            switch (item.Type)
            {
                case ConfigDataType.Int:
                case ConfigDataType.Float:
                    context.Append(string.Format(KeyValueFor, item.Name, item.DataList[dataIndex]));
                    break;
                case ConfigDataType.Vector2:
                case ConfigDataType.Vector3:
                case ConfigDataType.Vector4:
                    context.Append(FormatVectorValue(item.Name, item.DataList[dataIndex], layerCnt + 1));
                    break;
                default:
                    context.Append(string.Format(KeyValueFor, item.Name, string.Format(StringFor, item.DataList[dataIndex])));
                    break;
            }
            context.Append(",\n");
        }

        //格式化table
        private static void FormatTable(StringBuilder context, string key, bool isNumber, string tableContent, int layerCnt)
        {
            string layerStr = "";
            for (int i = 0; i < layerCnt; i++)
            {
                layerStr += "\t";
            }
            if (isNumber)
            {
                context.Append(string.Format(TableFor, layerStr, string.Format(KeyFor, key), tableContent, layerStr));
            }
            else
            {
                context.Append(string.Format(TableFor, layerStr, key, tableContent, layerStr));
            }
        }

        public static void ConfigToLua(Config config)
        {
            if (config.Items.Count <= 0)
            {
                return;
            }

            //table声明
            StringBuilder context = new StringBuilder("");

            //内容生成
            StringBuilder dataContent = new StringBuilder("");
            ConfigItem configKeyItem = config.Items[0];

            for (int i = 0; i < configKeyItem.DataList.Count; i++)
            {
                StringBuilder valueContent = new StringBuilder("");
                for (int j = 1; j < config.Items.Count; j++)
                {
                    FormatKeyValue(valueContent, config.Items[j], i, 2);
                }
                bool isNumber = configKeyItem.Type == ConfigDataType.Int || configKeyItem.Type == ConfigDataType.Float ? true : false;
                FormatTable(dataContent, configKeyItem.DataList[i], isNumber, valueContent.ToString(), 1);
                dataContent.Append(",\n");
            }

            //组装table
            FormatTable(context, config.Name, false, dataContent.ToString(), 0);

            //生成文件
            string path = CreateLuaFile(config.Name);
            EDTool.WriteText(context.ToString(), path);
        }

        public static void ConfigGroupToLua(ConfigGroup configGroup)
        {
            foreach (var item in configGroup.Configs)
            {
                ConfigToLua(item);
            }
        }
    }
}
