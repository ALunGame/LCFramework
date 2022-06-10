using LCToolkit;
using LCToolkit.ViewModel;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace LCNode.Model
{
    public abstract partial class BaseNode : ViewModel, IGraphElement
    {
        #region Fields
        [NonSerialized] BaseGraph owner;
        [NonSerialized] Dictionary<string, BasePort> ports;

        /// <summary>
        /// 端口添加事件
        /// </summary>
        public event Action<BasePort> onPortAdded;

        /// <summary>
        /// 端口移除事件
        /// </summary>
        public event Action<BasePort> onPortRemoved;
        #endregion

        #region Properties
        public BaseGraph Owner
        {
            get { return owner; }
        }
        public string GUID
        {
            get { return guid; }
        }
        public IReadOnlyDictionary<string, BasePort> Ports
        {
            get { return ports; }
        }
        public Vector2 Position
        {
            get { return GetPropertyValue<Vector2>(POSITION_NAME); }
            internal set { SetPropertyValue(POSITION_NAME, value); }
        }
        #endregion

        #region Virtual

        [NonSerialized]
        public Action<string> OnTitleChanged;
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

        internal void Enable(BaseGraph graph)
        {
            owner = graph;
            ports = new Dictionary<string, BasePort>();
            InitPort();
            //位置绑定
            this[POSITION_NAME] = new BindableProperty<Vector2>(() => position, v => position = v);

            OnEnabled();
        }

        private void InitPort()
        {
            //属性端口
            foreach (FieldInfo item in ReflectionHelper.GetFieldInfos(GetType()))
            {
                if (AttributeHelper.TryGetFieldAttribute(item, out NodeValueAttribute nodeValueAttribute))
                    continue;
                BasePort port = null;
                if (AttributeHelper.TryGetFieldAttribute(item, out InputPortAttribute inputAttr))
                    port = new BasePort(inputAttr.name, inputAttr.orientation, inputAttr.direction, inputAttr.capacity, item.FieldType, inputAttr.setIndex);
                if (AttributeHelper.TryGetFieldAttribute(item, out OutputPortAttribute outputAttr))
                    port = new BasePort(outputAttr.name, outputAttr.orientation, outputAttr.direction, outputAttr.capacity, item.FieldType, outputAttr.setIndex);
                if (port != null)
                {
                    AddPort(port);
                }
            }
        }

        internal void Initialize()
        {
            OnInitialized();
        }

        #region API
        public IEnumerable<BaseNode> GetConnections(string portName)
        {
            if (!Ports.TryGetValue(portName, out var port))
                yield break;
            if (port.direction == BasePort.Direction.Input)
            {
                foreach (var connection in port.Connections)
                {
                    yield return connection.FromNode;
                }
            }
            else
            {
                foreach (var connection in port.Connections)
                {
                    yield return connection.ToNode;
                }
            }
        }

        public void AddPort(BasePort port)
        {
            if (ports.ContainsKey(port.name))
            {
                throw new ArgumentException($"Already contains port:{port.name}");
            }
            ports[port.name] = port;
            port.Enable(this);
            onPortAdded?.Invoke(port);
        }

        public void RemovePort(string portName)
        {
            RemovePort(ports[portName]);
        }

        public void RemovePort(BasePort port)
        {
            if (port.Owner != this)
            {
                return;
            }
            if (!ports.ContainsKey(port.name))
            {
                throw new ArgumentException($"Not contains port:{port.name}");
            }
            Owner.Disconnect(port);
            ports.Remove(port.name);
            onPortRemoved?.Invoke(port);
        }
        #endregion

        #region Overrides
        protected virtual void OnEnabled()
        {

        }

        protected virtual void OnInitialized()
        {

        }
        #endregion

        #region 常量
        public const string POSITION_NAME = nameof(Position);
        #endregion

        #region 静态
        /// <summary> 根据T创建一个节点，并设置位置 </summary>
        public static T CreateNew<T>(BaseGraph graph, Vector2 position) where T : BaseNode
        {
            return CreateNew(graph, typeof(T), position) as T;
        }

        /// <summary> 根据type创建一个节点，并设置位置 </summary>
        public static BaseNode CreateNew(BaseGraph graph, Type type, Vector2 position)
        {
            if (!type.IsSubclassOf(typeof(BaseNode)))
                return null;
            var node = Activator.CreateInstance(type) as BaseNode;
            node.position = position;
            IDAllocation(node, graph);
            return node;
        }

        /// <summary> 给节点分配一个GUID，这将会覆盖已有GUID </summary>
        public static void IDAllocation(BaseNode node, BaseGraph graph)
        {
            node.guid = graph.GenerateNodeGUID();
        }
        #endregion

    }
}
