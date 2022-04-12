using System;
using UnityEngine;

namespace LCNode.Model
{
    public partial class BasePort
    {
        /// <summary>
        /// 输入输出
        /// </summary>
        public enum Direction {
            /// <summary>
            /// 输入
            /// </summary>
            Input,
            /// <summary>
            /// 输出
            /// </summary>
            Output
        }

        /// <summary>
        /// 排列方式
        /// </summary>
        public enum Orientation { 
            /// <summary>
            /// 水平
            /// </summary>
            Horizontal,
            /// <summary>
            /// 竖直
            /// </summary>
            Vertical
        }

        /// <summary>
        /// 数量
        /// </summary>
        public enum Capacity { 
            /// <summary>
            /// 单个
            /// </summary>
            Single,
            /// <summary>
            /// 多个
            /// </summary>
            Multi
        }

        public readonly string name;
        public readonly Orientation orientation;
        public readonly Direction direction;
        public readonly Capacity capacity;

        //端口值类型
        public readonly Type type;

        public BasePort(string name, Orientation orientation, Direction direction, Capacity capacity, Type type = null)
        {
            this.name = name;
            this.orientation = orientation;
            this.direction = direction;
            this.capacity = capacity;
            this.type = type == null ? typeof(object) : type;
        }
    }
}
