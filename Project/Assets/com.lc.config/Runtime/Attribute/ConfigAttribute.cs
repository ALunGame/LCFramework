using System;

namespace LCConfig
{
    /// <summary>
    /// 配置
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class ConfigAttribute : Attribute
    {
        /// <summary>
        /// 字段名
        /// </summary>
        public string DisplayName;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="displayName">注释名</param>
        public ConfigAttribute(string displayName)
        {
            this.DisplayName = displayName;
        }
    }

    /// <summary>
    /// 声明为配置键
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
    public class ConfigKeyAttribute : ConfigValueAttribute
    {
        /// <summary>
        /// 第几个键
        /// </summary>
        public int keyIndex = 1;

        /// <param name="keyIndex">第几个键</param>
        /// <param name="name">字段名</param>
        /// <param name="tooltip">字段提示</param>
        public ConfigKeyAttribute(int keyIndex, string name = "", string tooltip = "")
        {
            this.keyIndex = keyIndex;
            this.Name = name;
            this.Tooltip = tooltip;
        }
    }

    /// <summary>
    /// 配置值
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
    public class ConfigValueAttribute : Attribute
    {
        /// <summary>
        /// 字段名
        /// </summary>
        public string Name;
        /// <summary>
        /// 字段提示
        /// </summary>
        public string Tooltip;

        /// <param name="name">字段名</param>
        /// <param name="tooltip">字段提示</param>
        public ConfigValueAttribute(string name = "", string tooltip = "")
        {
            this.Name = name;
            this.Tooltip = tooltip;
        }
    }

    /// <summary>
    /// 配置忽略
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
    public class ConfigIgnoreAttribute : Attribute
    {
    }
}
