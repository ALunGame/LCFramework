using LCECS.Core.Tree.Base;

namespace LCECS.Model
{
    /// <summary>
    /// 决策树配置
    /// </summary>
    public class DecisionModel
    {
        /// <summary>
        /// 决策树Id
        /// </summary>
        public int id;

        /// <summary>
        /// 树
        /// </summary>
        public Node tree;
    }

    /// <summary>
    /// 行为树配置
    /// </summary>
    public class BehaviorModel
    {
        /// <summary>
        /// 行为树Id
        /// </summary>
        public int id;

        /// <summary>
        /// 树
        /// </summary>
        public Node tree;
    }
    
}
