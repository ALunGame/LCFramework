using MemoryPack;

using System;
using LCUI;

namespace Demo
{
    
    /// <summary>
    /// 界面配置
    /// </summary>
    [MemoryPackable]
    public partial class UIPanelCnf
    {
        
        /// <summary>
        /// 界面Id
        /// </summary>
        public UIPanelDef id;

        /// <summary>
        /// 预制体
        /// </summary>
        public string prefab;

        /// <summary>
        /// 脚本
        /// </summary>
        public string script;

        /// <summary>
        /// 界面层级
        /// </summary>
        public UILayer layer;

        /// <summary>
        /// UI画布类型
        /// </summary>
        public UICanvasType canvas;

        /// <summary>
        /// 默认显示规则
        /// </summary>
        public UIShowRule showRule;

    }

}

