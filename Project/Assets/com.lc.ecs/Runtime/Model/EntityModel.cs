using LCECS.Core;
using System.Collections.Generic;

namespace LCECS.Model
{
    /// <summary>
    /// 实体配置
    /// </summary>
    public class EntityModel
    {
        /// <summary>
        /// 实体Id
        /// </summary>
        public int id = 0;

        /// <summary>
        /// 实体名
        /// </summary>
        public string name = "";

        /// <summary>
        /// 决策树Id
        /// </summary>
        public int decTreeId;

        /// <summary>
        /// 预制体路径
        /// </summary>
        public string prefabPath = "";

        /// <summary>
        /// 组件列表
        /// </summary>
        public List<BaseCom> coms = new List<BaseCom>();
    }
}