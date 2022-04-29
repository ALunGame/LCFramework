using UnityEngine;
using UnityEditor.Experimental.GraphView;
using LCNode.Model;
using UnityEditor;

namespace LCNode.View.Utils
{
    /// <summary>
    /// 连接监听
    /// </summary>
    public class EdgeConnectorListener : IEdgeConnectorListener
    {
        private BaseGraphView graphView;
        private CreateNodeMenuWindow edgeNodeCreateMenuWindow;

        public EdgeConnectorListener(BaseGraphView graphView)
        {
            this.graphView = graphView;
        }

        /// <summary> 拖拽到符合条件的接口上松开时触发 </summary>
        public virtual void OnDrop(GraphView graphView, Edge edge)
        {
            BaseGraphView tempGraphView = graphView as BaseGraphView;

            BaseNode from = (edge.output.node as BaseNodeView).Model;
            BasePort fromPort = (edge.output as BasePortView).Model;
            BaseNode to = (edge.input.node as BaseNodeView).Model;
            BasePort toPort = (edge.input as BasePortView).Model;
            // 如果连线不是一个新建的连线就重定向
            if (edge.userData is BaseConnection connection)
                tempGraphView.CommandDispacter.Do(new ConnectionRedirectCommand(tempGraphView.Model, connection, from, fromPort.name, to, toPort.name));
            else
                tempGraphView.CommandDispacter.Do(new ConnectCommand(tempGraphView.Model, from, fromPort.name, to, toPort.name));
        }

        /// <summary> 拖到空白松开时触发 </summary>
        public void OnDropOutsidePort(Edge edge, Vector2 position) 
        {
            BaseConnectionView connectionView = edge as BaseConnectionView;
            if (!edge.isGhostEdge)
            {
                if (connectionView.Model!=null)
                {
                    graphView.Model.Disconnect(connectionView.Model);
                }
            }

            if (edge.input == null || edge.output == null)
            {
                ShowNodeCreationMenuFromEdge(connectionView, position);
            }
        }

        private void ShowNodeCreationMenuFromEdge(BaseConnectionView connectionView, Vector2 position)
        {
            if (edgeNodeCreateMenuWindow == null)
                edgeNodeCreateMenuWindow = ScriptableObject.CreateInstance<CreateNodeMenuWindow>();
            edgeNodeCreateMenuWindow.Initialize(graphView, graphView.GetNodeTypes());
            edgeNodeCreateMenuWindow.ConnectionFilter = connectionView;
            SearchWindow.Open(new SearchWindowContext(position + EditorWindow.focusedWindow.position.position), edgeNodeCreateMenuWindow);
        }
    }
}
