using System;
using UnityEngine;

namespace LCNode.Model
{
    public partial class BaseNode
    {
        #region NonSerialized
        
        [NonSerialized]
        public BaseGraphVM owner;
        
        public BaseGraphVM Owner
        {
            get { return owner; }
        }
        
        [NonSerialized]
        public Action<string> OnTitleChanged;
        
        [NonSerialized]
        private string title = "";
        /// <summary>
        /// 节点标题名
        /// </summary>
        public virtual string Title
        {
            get { return title; }
            set { 
                title = value;
                OnTitleChanged?.Invoke(title);
            }
        }
        
        [NonSerialized]
        public Action<Color> OnTitleColorChanged;
        [NonSerialized]
        private Color titleColor = Color.white;
        /// <summary>
        /// 节点标题颜色
        /// </summary>
        public virtual Color TitleColor
        {
            get { return titleColor; }
            set
            {
                titleColor = value;
                OnTitleColorChanged?.Invoke(titleColor);
            }
        }
        
        [NonSerialized]
        public Action<string> OnTooltipChanged;
        [NonSerialized]
        private string tooltip = "";
        /// <summary>
        /// 节点悬浮提示
        /// </summary>
        public virtual string Tooltip
        {
            get { return tooltip; }
            set
            {
                tooltip = value;
                OnTooltipChanged?.Invoke(tooltip);
            }
        }

        #endregion
        
        /// <summary> 唯一标识 </summary>
        internal string guid;
        /// <summary> 位置坐标 </summary>
        internal Vector2 position;
        /// <summary> 输入索引 </summary>
        internal int inIndex = -1;
        /// <summary> 输出索引 </summary>
        internal int outIndex = -1;
    }
}
