
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using LCNode.View.Utils;

using UnityObject = UnityEngine.Object;
using LCNode.Model;
using LCToolkit.Command;
using LCToolkit.Core;

namespace LCNode.View
{
    /// <summary>
    /// 视图显示
    /// </summary>
    public partial class BaseGraphView : GraphView, IBindableView<BaseGraph>
    {
        #region 属性
        public event Action onDirty;
        public event Action onUndirty;

        public CreateNodeMenuWindow CreateNodeMenu { get; private set; }
        public BaseGraphWindow GraphWindow { get; private set; }
        public CommandDispatcher CommandDispacter { get; private set; }
        public UnityObject GraphAsset { get { return GraphWindow.GraphAsset; } }
        public Dictionary<string, BaseNodeView> NodeViews { get; private set; } = new Dictionary<string, BaseNodeView>();

        public BaseGraph Model { get; set; }

        #endregion

        public BaseGraphView()
        {
            styleSheets.Add(GraphProcessorStyles.GraphViewStyle);

            Insert(0, new GridBackground());

            this.AddManipulator(new ContentZoomer() { minScale = 0.05f, maxScale = 2f });
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.StretchToParentSize();
        }

        #region Initialize
        public void SetUp(BaseGraph graph, BaseGraphWindow window, CommandDispatcher commandDispacter)
        {
            Model = graph;
            GraphWindow = window;
            CommandDispacter = commandDispacter;
            EditorCoroutine coroutine = GraphWindow.StartCoroutine(Initialize());
            RegisterCallback<DetachFromPanelEvent>(evt => { GraphWindow.StopCoroutine(coroutine); });
        }

        IEnumerator Initialize()
        {
            // 初始化
            viewTransform.position = Model.Pos == default ? Vector3.zero : Model.Pos;
            viewTransform.scale = Model.Zoom == default ? Vector3.one : Model.Zoom;

            // 绑定
            BindingProperties();
            RegisterCallback<DetachFromPanelEvent>(evt => { UnBindingProperties(); });

            InitializeCallbacks();

            yield return GraphWindow.StartCoroutine(GenerateNodeViews());
            yield return GraphWindow.StartCoroutine(LinkNodeViews());

            UpdateInspector();
            OnInitialized();
        }

        void InitializeCallbacks()
        {
            graphViewChanged = GraphViewChangedCallback;
            viewTransformChanged = ViewTransformChangedCallback;

            CreateNodeMenu = ScriptableObject.CreateInstance<CreateNodeMenuWindow>();
            CreateNodeMenu.Initialize(this, GetNodeTypes());
            nodeCreationRequest = c => SearchWindow.Open(new SearchWindowContext(c.screenMousePosition), CreateNodeMenu);
        }

        /// <summary> 生成所有NodeView </summary>
        IEnumerator GenerateNodeViews()
        {
            int step = 0;
            foreach (var node in Model.Nodes.Values)
            {
                if (node == null) continue;
                AddNodeView(node);
                step++;
                if (step % 5 == 0)
                    yield return null;
            }
        }

        /// <summary> 连接节点 </summary>
        IEnumerator LinkNodeViews()
        {
            int step = 0;
            foreach (var connection in Model.Connections)
            {
                if (connection == null) continue;
                BaseNodeView fromNodeView, toNodeView;
                if (!NodeViews.TryGetValue(connection.FromNodeGUID, out fromNodeView)) throw new NullReferenceException($"找不到From节点{connection.FromNodeGUID}");
                if (!NodeViews.TryGetValue(connection.ToNodeGUID, out toNodeView)) throw new NullReferenceException($"找不到To节点{connection.ToNodeGUID}");
                ConnectView(fromNodeView, toNodeView, connection);
                step++;
                if (step % 5 == 0)
                    yield return null;
            }
        }
        #endregion

        #region 数据监听回调
        void OnPositionChanged(Vector3 position)
        {
            viewTransform.position = position;
            SetDirty();
        }

        void OnScaleChanged(Vector3 scale)
        {
            viewTransform.scale = scale;
            SetDirty();
        }

        void OnNodeAdded(BaseNode node)
        {
            AddNodeView(node);
            SetDirty();
        }

        void OnNodeRemoved(BaseNode node)
        {
            RemoveNodeView(NodeViews[node.GUID]);
            SetDirty();
        }

        void OnConnected(BaseConnection connection)
        {
            var from = NodeViews[connection.FromNodeGUID];
            var to = NodeViews[connection.ToNodeGUID];
            ConnectView(from, to, connection);
            SetDirty();
        }

        void OnDisconnected(BaseConnection connection)
        {
            edges.ForEach(edge =>
            {
                if (edge.userData != connection) return;
                DisconnectView(edge as BaseConnectionView);
            });
            SetDirty();
        }

        protected virtual void BindingProperties()
        {
            Model.BindingProperty<Vector3>(BaseGraph.POS_NAME, OnPositionChanged);
            Model.BindingProperty<Vector3>(BaseGraph.ZOOM_NAME, OnScaleChanged);

            Model.onNodeAdded += OnNodeAdded;
            Model.onNodeRemoved += OnNodeRemoved;

            Model.onConnected += OnConnected;
            Model.onDisconnected += OnDisconnected;
        }

        public void UnBindingProperties()
        {
            this.Query<GraphElement>().ForEach(element =>
            {
                if (element is IBindableView bindableView)
                {
                    bindableView.UnBindingProperties();
                }
            });

            Model.ClearBindingEvent();

            Model.onNodeAdded -= OnNodeAdded;
            Model.onNodeRemoved -= OnNodeRemoved;

            Model.onConnected -= OnConnected;
            Model.onDisconnected -= OnDisconnected;
        }
        #endregion

        #region 回调方法
        /// <summary> GraphView发生改变时调用 </summary>
        GraphViewChange GraphViewChangedCallback(GraphViewChange changes)
        {
            if (changes.movedElements != null)
            {
                CommandDispacter.BeginGroup();
                // 当节点移动之后，与之连接的接口重新排序
                Dictionary<BaseNode, Vector2> newPos = new Dictionary<BaseNode, Vector2>();
                HashSet<BasePort> ports = new HashSet<BasePort>();
                changes.movedElements.RemoveAll(element =>
                {
                    switch (element)
                    {
                        case BaseNodeView nodeView:
                            newPos[nodeView.Model] = nodeView.GetPosition().position;
                            // 记录需要重新排序的接口
                            foreach (var port in nodeView.Model.Ports.Values)
                            {
                                foreach (var connection in port.Connections)
                                {
                                    if (port.direction == BasePort.Direction.Input)
                                    {
                                        ports.Add(connection.FromNode.Ports[connection.FromPortName]);
                                    }
                                    else
                                    {
                                        ports.Add(connection.ToNode.Ports[connection.ToPortName]);
                                    }
                                }
                            }
                            return true;
                        default:
                            break;
                    }
                    return false;
                });

                CommandDispacter.Do(new MoveNodesCommand(newPos));
                // 排序
                foreach (var port in ports)
                {
                    port.Resort();
                }
                CommandDispacter.EndGroup();
            }
            if (changes.elementsToRemove != null)
            {
                changes.elementsToRemove.Sort((element1, element2) =>
                {
                    int GetPriority(GraphElement element)
                    {
                        switch (element)
                        {
                            case Edge edgeView:
                                return 0;
                            case BaseNodeView nodeView:
                                return 1;
                        }
                        return 4;
                    }
                    return GetPriority(element1).CompareTo(GetPriority(element2));
                });
                CommandDispacter.BeginGroup();
                changes.elementsToRemove.RemoveAll(element =>
                {
                    switch (element)
                    {
                        case BaseConnectionView edgeView:
                            if (edgeView.selected)
                                CommandDispacter.Do(new DisconnectCommand(Model, edgeView.Model));
                            return true;
                        case BaseNodeView nodeView:
                            if (nodeView.selected)
                                CommandDispacter.Do(new RemoveNodeCommand(Model, nodeView.Model));
                            return true;
                    }
                    return false;
                });
                CommandDispacter.EndGroup();

                UpdateInspector();
            }
            return changes;
        }

        /// <summary> 转换发生改变时调用 </summary>
        void ViewTransformChangedCallback(GraphView view)
        {
            Model.Pos = viewTransform.position;
            Model.Zoom = viewTransform.scale;
        }

        public sealed override void AddToSelection(ISelectable selectable)
        {
            base.AddToSelection(selectable);
            UpdateInspector();
        }

        public sealed override void RemoveFromSelection(ISelectable selectable)
        {
            base.RemoveFromSelection(selectable);
            UpdateInspector();
        }

        public sealed override void ClearSelection()
        {
            base.ClearSelection();
            UpdateInspector();
        }

        #endregion

        #region 方法
        public BaseNodeView AddNodeView(BaseNode node)
        {
            Type nodeViewType = GetNodeViewType(node);
            BaseNodeView nodeView = Activator.CreateInstance(nodeViewType) as BaseNodeView;
            nodeView.SetUp(node, this);
            NodeViews[node.GUID] = nodeView;
            AddElement(nodeView);
            return nodeView;
        }

        public void RemoveNodeView(BaseNodeView nodeView)
        {
            RemoveElement(nodeView);
            NodeViews.Remove(nodeView.Model.GUID);
        }

        public BaseConnectionView ConnectView(BaseNodeView from, BaseNodeView to, BaseConnection connection)
        {
            var edgeView = Activator.CreateInstance(GetConnectionViewType(connection), true) as BaseConnectionView;
            edgeView.SetUp(connection, this);
            edgeView.userData = connection;
            edgeView.output = from.portViews[connection.FromPortName];
            edgeView.input = to.portViews[connection.ToPortName];
            from.portViews[connection.FromPortName].Connect(edgeView);
            to.portViews[connection.ToPortName].Connect(edgeView);
            AddElement(edgeView);
            return edgeView;
        }

        public void DisconnectView(BaseConnectionView edgeView)
        {
            BasePortView inputPortView = edgeView.input as BasePortView;
            BaseNodeView inputNodeView = inputPortView.node as BaseNodeView;
            if (inputPortView != null)
            {
                inputPortView.Disconnect(edgeView);
            }
            inputPortView.Disconnect(edgeView);

            BasePortView outputPortView = edgeView.output as BasePortView;
            BaseNodeView outputNodeView = outputPortView.node as BaseNodeView;
            if (outputPortView != null)
            {
                outputPortView.Disconnect(edgeView);
            }
            outputPortView.Disconnect(edgeView);

            inputNodeView.RefreshPorts();
            outputNodeView.RefreshPorts();

            RemoveElement(edgeView);
        }

        /// <summary> 获取鼠标在GraphView中的坐标，如果鼠标不在GraphView内，则返回当前GraphView显示的中心点 </summary>
        public Vector2 GetMousePosition()
        {
            if (worldBound.Contains(Event.current.mousePosition))
                return contentViewContainer.WorldToLocal(Event.current.mousePosition);
            return contentViewContainer.WorldToLocal(worldBound.center);
        }

        // 标记Dirty
        public void SetDirty()
        {
            onDirty?.Invoke();
        }

        public void SetUndirty()
        {
            onUndirty?.Invoke();
        }
        #endregion
    }
}
