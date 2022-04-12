using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LCToolkit.ViewModel;
using LCJson;

namespace LCNode.Model
{
    public partial class BaseConnection : ViewModel, IGraphElement
    {
        #region Fields
        [NonSerialized] BaseGraph owner;
        [NonSerialized] BaseNode fromNode;
        [NonSerialized] BaseNode toNode;
        #endregion

        #region Properties
        public BaseGraph Owner { get { return owner; } }
        public string FromNodeGUID { get { return from; } }
        public string ToNodeGUID { get { return to; } }
        public string FromPortName { get { return fromPortName; } }
        public string ToPortName { get { return toPortName; } }
        public BaseNode FromNode { get { return fromNode; } }
        public BaseNode ToNode { get { return toNode; } }
        #endregion

        internal void Enable(BaseGraph graph)
        {
            owner = graph;
            owner.Nodes.TryGetValue(FromNodeGUID, out fromNode);
            owner.Nodes.TryGetValue(ToNodeGUID, out toNode);
            OnEnabled();
        }

        /// <summary>
        /// 重定向
        /// </summary>
        internal void Redirect(BaseNode from, string fromPortName, BaseNode to, string toPortName)
        {
            this.from = from.GUID;
            this.fromPortName = fromPortName;

            this.to = to.GUID;
            this.toPortName = toPortName;

            Enable(owner);
        }

        /// <summary>
        /// 获得From或者To端口
        /// </summary>
        /// <returns></returns>
        public (BasePort, BasePort) GetInOrOutPort()
        {
            (BasePort, BasePort) item;
            item.Item1 = FromNode != null ? FromNode.Ports[FromPortName] : null;
            item.Item2 = ToNode != null ? ToNode.Ports[ToPortName] : null;
            return item;
        }

        #region Overrides
        protected virtual void OnEnabled() { }
        #endregion

        #region Static
        public static T CreateNew<T>(BaseNode from, string fromPortName, BaseNode to, string toPortName) where T : BaseConnection
        {
            return CreateNew(typeof(T), from, fromPortName, to, toPortName) as T;
        }

        public static BaseConnection CreateNew(Type type, BaseNode from, string fromPortName, BaseNode to, string toPortName)
        {
            var connection = Activator.CreateInstance(type) as BaseConnection;
            connection.fromNode = from;
            connection.from = from.GUID;
            connection.fromPortName = fromPortName;
            connection.toNode = to;
            connection.to = to.GUID;
            connection.toPortName = toPortName;
            return connection;
        }
        #endregion

    }
}
