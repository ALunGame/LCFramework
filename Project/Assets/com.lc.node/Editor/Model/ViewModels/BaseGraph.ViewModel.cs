using LCToolkit;
using LCToolkit.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LCNode.Model
{
    public abstract partial class BaseGraph : ViewModel
    {
        /// <summary>
        /// 节点命名空间限制
        /// </summary>
        public virtual List<string> NodeNamespace => null;

        #region Fields
        public event Action<BaseNode> onNodeAdded;
        public event Action<BaseNode> onNodeRemoved;

        public event Action<BaseConnection> onConnected;
        public event Action<BaseConnection> onDisconnected;
        
        public event Action<BaseGroup> OnGroupAdded;
        public event Action<BaseGroup> OnGroupRemoved;
        #endregion

        #region Properties
        public Vector3 Pos
        {
            get { return GetPropertyValue<Vector3>(POS_NAME); }
            set { SetPropertyValue(POS_NAME, value); }
        }
        public Vector3 Zoom
        {
            get { return GetPropertyValue<Vector3>(ZOOM_NAME); }
            set { SetPropertyValue(ZOOM_NAME, value); }
        }
        public IReadOnlyDictionary<string, BaseNode> Nodes
        {
            get { return nodes; }
        }
        public IReadOnlyList<BaseGroup> Groups
        {
            get { return groups; }
        }
        public IReadOnlyList<BaseConnection> Connections
        {
            get { return connections; }
        }
        #endregion

        public void Enable()
        {
            foreach (var node in Nodes.Values)
            {
                node.Enable(this);
            }
            for (int i = 0; i < connections.Count; i++)
            {
                var connection = connections[i];
                if (connection == null)
                {
                    connections.RemoveAt(i--);
                    continue;
                }
                if (!nodes.TryGetValue(connection.FromNodeGUID, out var fromNode))
                {
                    connections.RemoveAt(i--);
                    continue;
                }
                if (!nodes.TryGetValue(connection.ToNodeGUID, out var toNode))
                {
                    connections.RemoveAt(i--);
                    continue;
                }
                if (!fromNode.Ports.TryGetValue(connection.FromPortName, out var fromPort))
                {
                    connections.RemoveAt(i--);
                    continue;
                }
                if (!toNode.Ports.TryGetValue(connection.ToPortName, out var toPort))
                {
                    connections.RemoveAt(i--);
                    continue;
                }

                connection.Enable(this);

                fromPort.ConnectTo(connection);
                toPort.ConnectTo(connection);
            }

            this[POS_NAME] = new BindableProperty<Vector3>(() => pos, v => pos = v);
            this[ZOOM_NAME] = new BindableProperty<Vector3>(() => zoom, v => zoom = v);

            for (int i = 0; i < groups.Count; i++)
            {
                var group = groups[i];
                if (group == null)
                {
                    groups.RemoveAt(i--);
                    continue;
                }
                group.Enable(this);
                groups.Add(group);
            }
            
            OnEnabled();
        }

        #region API

        //生成唯一Id
        public string GenerateNodeGUID()
        {
            while (true)
            {
                string guid = Guid.NewGuid().ToString();
                if (!nodes.ContainsKey(guid)) return guid;
            }
        }

        //添加节点
        public void AddNode(BaseNode node)
        {
            if (node.Owner != null && node.Owner != this)
                throw new Exception("节点存在其它Graph中");
            if (node.ContainsKey(node.GUID))
                throw new Exception("节点添加失败，GUID重复");

            node.Enable(this);
            nodes[node.GUID] = node;
            onNodeAdded?.Invoke(node);
        }

        //添加节点
        public T AddNode<T>(Vector2 position) where T : BaseNode
        {
            T node = BaseNode.CreateNew<T>(this, position);
            AddNode(node);
            return node;
        }

        //添加节点
        public BaseNode AddNode(Type type, Vector2 position)
        {
            BaseNode node = BaseNode.CreateNew(this, type, position);
            AddNode(node);
            return node;
        }

        //移除节点
        public void RemoveNode(BaseNode node)
        {
            if (node == null)
                throw new NullReferenceException("节点不能为空");
            Disconnect(node);
            nodes.Remove(node.GUID);
            onNodeRemoved?.Invoke(node);
        }

        //连接
        public bool Connect(BaseConnection connection)
        {
            if (!Nodes.TryGetValue(connection.FromNodeGUID, out var fromNode))
                return false;
            if (!fromNode.Ports.TryGetValue(connection.FromPortName, out var fromPort))
                return false;

            if (!Nodes.TryGetValue(connection.ToNodeGUID, out var toNode))
                return false;
            if (!toNode.Ports.TryGetValue(connection.ToPortName, out var toPort))
                return false;

            var tmpConnection = fromPort.Connections.FirstOrDefault(tmp => tmp.ToNodeGUID == connection.ToNodeGUID && tmp.ToPortName == connection.ToPortName);
            if (tmpConnection != null)
                return false;

            if (fromPort.capacity == BasePort.Capacity.Single)
                Disconnect(fromPort);
            if (toPort.capacity == BasePort.Capacity.Single)
                Disconnect(toPort);

            connection.Enable(this);
            connections.Add(connection);

            fromPort.ConnectTo(connection);
            toPort.ConnectTo(connection);

            onConnected?.Invoke(connection);
            return true;
        }

        //连接
        public BaseConnection Connect(BaseNode from, string fromPortName, BaseNode to, string toPortName)
        {
            var connection = NewConnection(from, fromPortName, to, toPortName);
            if (!Connect(connection))
                return null;
            return connection;
        }

        //断开连接
        public void Disconnect(BaseNode node)
        {
            foreach (var connection in Connections.ToArray())
            {
                if (connection.FromNodeGUID == node.GUID || connection.ToNodeGUID == node.GUID)
                    Disconnect(connection);
            }
        }

        //断开连接
        public void Disconnect(BaseConnection connection)
        {
            if (!connections.Contains(connection)) return;

            connection.FromNode.Ports.TryGetValue(connection.FromPortName, out BasePort fromPort);
            fromPort.DisconnectTo(connection);

            connection.ToNode.Ports.TryGetValue(connection.ToPortName, out BasePort toPort);
            toPort.DisconnectTo(connection);

            connections.Remove(connection);
            onDisconnected?.Invoke(connection);
        }

        //断开连接
        public void Disconnect(BasePort port)
        {
            if (port.Owner == null || !nodes.ContainsKey(port.Owner.GUID))
                return;
            foreach (var connection in port.Connections.ToArray())
            {
                Disconnect(connection);
            }
        }

        //断开连接
        public void Disconnect(BaseNode node, string portName)
        {
            Disconnect(node.Ports[portName]);
        }
        
        public void AddGroup(BaseGroup group)
        {
            if (groups.Contains(group))
                return;
            groups.Add(group);
            group.Enable(this);
            OnGroupAdded?.Invoke(group);
        }
        

        public void RemoveGroup(BaseGroup group)
        {
            bool removed = groups.Remove(group);
            groups.Remove(group);
            if (removed)
                OnGroupRemoved?.Invoke(group);
        }
        
        #endregion

        #region Overrides
        protected virtual void OnEnabled() { }

        protected virtual void OnInitialized() { }

        public T NewNode<T>(Vector2 position) where T : BaseNode
        {
            return NewNode(typeof(T), position) as T;
        }

        public virtual BaseNode NewNode(Type type, Vector2 position)
        {
            return BaseNode.CreateNew(this, type, position);
        }

        public virtual BaseConnection NewConnection(BaseNode from, string fromPortName, BaseNode to, string toPortName)
        {
            return BaseConnection.CreateNew<BaseConnection>(from, fromPortName, to, toPortName);
        }

        public virtual IEnumerable<Type> GetNodeTypes()
        {
            foreach (var type in ReflectionHelper.GetChildTypes<BaseNode>())
            {
                if (type.IsAbstract)
                    continue;
                if (!AttributeHelper.TryGetTypeAttribute(type, out NodeMenuItemAttribute attr))
                    continue;
                if (NodeNamespace != null && !NodeNamespace.Contains(type.Namespace))
                    continue;
                yield return type;
            }
        }

        #endregion

        #region Static
        public const string POS_NAME = nameof(pos);
        public const string ZOOM_NAME = nameof(zoom);
        #endregion
    }
}
