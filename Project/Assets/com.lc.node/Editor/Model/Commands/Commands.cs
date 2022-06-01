using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using LCToolkit.Command;
using System.Reflection;
using LCToolkit.ViewModel;

namespace LCNode.Model
{
    /// <summary>
    /// 添加节点命令
    /// </summary>
    public class AddNodeCommand : ICommand
    {
        BaseGraph graph;
        BaseNode node;
        public AddNodeCommand(BaseGraph graph, BaseNode node)
        {
            this.graph = graph;
            this.node = node;
        }

        public void Do()
        {
            graph.AddNode(node);
        }

        public void Undo()
        {
            graph.RemoveNode(node);
        }
    }

    /// <summary>
    /// 删除节点命令
    /// </summary>
    public class RemoveNodeCommand : ICommand
    {
        BaseGraph graph;
        BaseNode node;

        List<BaseConnection> connections = new List<BaseConnection>();
        public RemoveNodeCommand(BaseGraph graph, BaseNode node)
        {
            this.graph = graph;
            this.node = node;
        }

        public void Do()
        {
            foreach (var edge in graph.Connections.ToArray())
            {
                if (edge.FromNode == node || edge.ToNode == node)
                {
                    connections.Add(edge);
                }
            }
            graph.RemoveNode(node);
        }

        public void Undo()
        {
            graph.AddNode(node);
            foreach (var edge in connections)
            {
                graph.Connect(edge);
            }
            connections.Clear();
        }
    }

    public class AddPortCommand : ICommand
    {
        BaseNode node;
        BasePort port;
        bool successed = false;

        public AddPortCommand(BaseNode node, string name, BasePort.Orientation orientation, BasePort.Direction direction, BasePort.Capacity capacity, Type type = null)
        {
            this.node = node;
            port = new BasePort(name, orientation, direction, capacity, type);
        }

        public void Do()
        {
            successed = false;
            if (!node.Ports.ContainsKey(port.name))
            {
                node.AddPort(port);
                successed = true;
            }
        }

        public void Undo()
        {
            if (!successed)
            {
                return;
            }
            node.RemovePort(port);
        }
    }

    /// <summary>
    /// 删除端口命令
    /// </summary>
    public class RemovePortCommand : ICommand
    {
        BaseNode node;
        BasePort port;
        bool successed = false;

        public RemovePortCommand(BaseNode node, BasePort port)
        {
            this.node = node;
            this.port = port;
        }

        public RemovePortCommand(BaseNode node, string name)
        {
            this.node = node;
            node.Ports.TryGetValue(name, out port);
        }

        public void Do()
        {
            successed = false;
            if (node.Ports.ContainsKey(port.name))
            {
                node.AddPort(port);
                successed = true;
            }
        }

        public void Undo()
        {
            if (!successed)
            {
                return;
            }
            node.RemovePort(port);
        }
    }

    /// <summary>
    /// 连接命令
    /// </summary>
    public class ConnectCommand : ICommand
    {
        private readonly BaseGraph graph;
        private readonly BaseNode from;
        private readonly string fromPortName;
        private readonly BaseNode to;
        private readonly string toPortName;

        BaseConnection connection;
        HashSet<BaseConnection> replacedConnections = new HashSet<BaseConnection>();

        public ConnectCommand(BaseGraph graph, BaseNode from, string fromPortName, BaseNode to, string toPortName)
        {
            this.graph = graph;
            this.from = from;
            this.fromPortName = fromPortName;
            this.to = to;
            this.toPortName = toPortName;
        }

        public ConnectCommand(BaseGraph graph, BaseConnection connection)
        {
            this.graph = graph;
            this.connection = connection;
            this.from = connection.FromNode;
            this.fromPortName = connection.FromPortName;
            this.to = connection.ToNode;
            this.toPortName = connection.ToPortName;
        }

        public void Do()
        {
            replacedConnections.Clear();
            if (from.Ports[fromPortName].capacity == BasePort.Capacity.Single)
            {
                foreach (var connection in from.Ports[fromPortName].Connections)
                {
                    replacedConnections.Add(connection);
                }
            }
            if (to.Ports[toPortName].capacity == BasePort.Capacity.Single)
            {
                foreach (var connection in to.Ports[toPortName].Connections)
                {
                    replacedConnections.Add(connection);
                }
            }

            if (connection == null)
            {
                connection = graph.Connect(from, fromPortName, to, toPortName);
            }
            else
            {
                graph.Connect(connection);
            }
        }

        public void Undo()
        {
            graph.Disconnect(connection);

            // 还原
            foreach (var connection in replacedConnections)
            {
                graph.Connect(connection);
            }
        }
    }

    /// <summary>
    /// 连接转向命令
    /// </summary>
    public class ConnectionRedirectCommand : ICommand
    {
        BaseGraph graph;
        BaseConnection connection;

        BaseNode oldFrom, oldTo;
        string oldFromPortName, oldToPortName;

        BaseNode newFrom, newTo;
        string newFromPortName, newToPortName;

        List<BaseConnection> replacedConnections = new List<BaseConnection>();

        public ConnectionRedirectCommand(BaseGraph graph, BaseConnection connection, BaseNode from, string fromPortName, BaseNode to, string toPortName)
        {
            this.graph = graph;
            this.connection = connection;

            newFrom = from;
            newFromPortName = fromPortName;
            newTo = to;
            newToPortName = toPortName;
        }

        public void Do()
        {
            oldFrom = connection.FromNode;
            oldFromPortName = connection.FromPortName;
            oldTo = connection.ToNode;
            oldToPortName = connection.ToPortName;

            replacedConnections.Clear();
            if (connection.FromNodeGUID == newFrom.GUID && connection.FromPortName == newFromPortName)
            {
                if (newTo.Ports[newToPortName].capacity == BasePort.Capacity.Single)
                    replacedConnections.AddRange(newTo.Ports[newToPortName].Connections);
            }
            else
            {
                if (newFrom.Ports[newFromPortName].capacity == BasePort.Capacity.Single)
                    replacedConnections.AddRange(newFrom.Ports[newFromPortName].Connections);
            }

            connection.Redirect(newFrom, newFromPortName, newTo, newToPortName);
            graph.Connect(connection);
        }

        public void Undo()
        {
            graph.Disconnect(connection);
            connection.Redirect(oldFrom, oldFromPortName, oldTo, oldToPortName);
            graph.Connect(connection);

            // 还原
            foreach (var connection in replacedConnections)
            {
                graph.Connect(connection);
            }
        }
    }

    /// <summary>
    /// 断开连接命令
    /// </summary>
    public class DisconnectCommand : ICommand
    {
        BaseGraph graph;

        BaseConnection connection;

        public DisconnectCommand(BaseGraph graph, BaseConnection connection)
        {
            this.graph = graph;
            this.connection = connection;
        }

        public void Do()
        {
            graph.Disconnect(connection);
        }

        public void Undo()
        {
            graph.Connect(connection);
        }
    }

    /// <summary>
    /// 移动节点命令
    /// </summary>
    public class MoveNodeCommand : ICommand
    {
        BaseNode node;
        Vector2 currentPosition;
        Vector2 targetPosition;

        public MoveNodeCommand(BaseNode node, Vector2 position)
        {
            this.node = node;
            currentPosition = node.Position;
            targetPosition = position;
        }

        public void Do()
        {
            node.Position = targetPosition;
        }

        public void Undo()
        {
            node.Position = currentPosition;
        }
    }

    /// <summary>
    /// 移动一片节点命令
    /// </summary>
    public class MoveNodesCommand : ICommand
    {
        Dictionary<BaseNode, Vector2> oldPos = new Dictionary<BaseNode, Vector2>();
        Dictionary<BaseNode, Vector2> newPos = new Dictionary<BaseNode, Vector2>();

        public MoveNodesCommand(Dictionary<BaseNode, Vector2> newPos)
        {
            this.newPos = newPos;
        }

        public void Do()
        {
            foreach (var pair in newPos)
            {
                oldPos[pair.Key] = pair.Key.Position;
                pair.Key.Position = pair.Value;
            }
        }

        public void Undo()
        {
            foreach (var pair in oldPos)
            {
                pair.Key.Position = pair.Value;
            }
        }
    }

    /// <summary>
    /// 改变节点值命令
    /// </summary>
    public class ChangeNodeValueCommand : ICommand
    {
        object target;
        FieldInfo field;
        object oldValue, newValue;

        Action OnDoFunc;
        Action OnUndoFunc;

        public ChangeNodeValueCommand(object target, FieldInfo field, object newValue, Action OnDoFunc = null, Action OnUndoFunc = null)
        {
            this.target = target;
            this.field = field;
            this.newValue = newValue;
            this.OnDoFunc = OnDoFunc;   
            this.OnUndoFunc = OnUndoFunc;
        }

        public void Do()
        {
            oldValue = field.GetValue(target);
            field.SetValue(target, newValue);
            OnDoFunc?.Invoke();
        }

        public void Undo()
        {
            field.SetValue(target, oldValue);
            OnUndoFunc?.Invoke();
        }
    }

    /// <summary>
    /// 绑定值改变命令
    /// </summary>
    public class BindableChangeValueCommand : ICommand
    {
        IBindableProperty property;
        object oldValue, newValue;

        public BindableChangeValueCommand(IBindableProperty property, object newValue)
        {
            this.property = property;
            this.newValue = newValue;
            this.oldValue = property.ValueBoxed;
        }

        public void Do()
        {
            property.ValueBoxed = newValue;
        }

        public void Undo()
        {
            property.ValueBoxed = oldValue;
        }
    }

    /// <summary>
    /// 值改变命令
    /// </summary>
    public class ChangeValueCommand : ICommand
    {
        object oldValue, newValue;
        Action<object> OnDoFunc;
        Action<object> OnUndoFunc;

        public ChangeValueCommand(object oldValue, object newValue, Action<object> onDoFunc, Action<object> onUndoFunc)
        {
            this.newValue = newValue;
            this.oldValue = oldValue;
            this.OnDoFunc = onDoFunc;
            this.OnUndoFunc = onUndoFunc;
        }

        public void Do()
        {
            OnDoFunc?.Invoke(newValue);
        }

        public void Undo()
        {
            OnUndoFunc?.Invoke(oldValue);
        }
    }
}
