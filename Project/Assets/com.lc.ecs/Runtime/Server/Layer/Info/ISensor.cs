using LCECS.Core;
using System;

namespace LCECS.Layer.Info
{
    /// <summary>
    /// 世界感知器 特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class WorldSensorAttribute : Attribute
    {
        /// <summary>
        /// 世界信息键
        /// </summary>
        public SensorType InfoKey { get; set; } = 0;

        public WorldSensorAttribute(SensorType infoKey)
        {
            this.InfoKey = infoKey;
        }
    }

    /// <summary>
    /// 世界感知器（收集世界信息）
    /// </summary>
    public interface ISensor
    {
    }
}
