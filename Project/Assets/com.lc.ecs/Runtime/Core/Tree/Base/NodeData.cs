
using System.Collections.Generic;

namespace LCECS.Core.Tree.Base
{
    /// <summary>
    /// 节点数据
    /// </summary>
    public class NodeData : AnyData
    {
        public string Uid = "";
        public Dictionary<string, NodeContext> Context;

        /// <summary>
        /// 黑板数据
        /// </summary>
        public Dictionary<string, object> Blackboard;

        public NodeData(string uId)
        {
            Uid = uId;
            Context = new Dictionary<string, NodeContext>();
            Blackboard = new Dictionary<string, object>();
        }

        ~NodeData()
        {
            Context = null;
            Blackboard = null;
        }
    }

    public class AnyData
    {
        public T As<T>() where T : AnyData
        {
            return (T)this;
        }
    }
}
