using LCECS.NodeGraph.Logic.Node;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XPToolchains.NodeGraph;

namespace Demo.NodeGraph.Decision
{
    public class DemoDecisionGraphWindow : DecisionGraphWindow
    {
        [MenuItem("决策/编辑")]
        public static DemoDecisionGraphWindow OpenWindow()
        {
            DemoDecisionGraphWindow window = GetWindow<DemoDecisionGraphWindow>();
            window.InitGraph(new List<string>() { "NodeGraph.Logic.Node", "Demo.NodeGraph.Decision", "Demo.NodeGraph.Premise" , "NodeGraph.Decision.Node" });
            window.titleContent = new GUIContent("决策编辑");
            window.Show();
            return window;
        }

        protected override void GetNodeId(BaseGraph graph, BaseNode node)
        {
            string nodeId = Guid.NewGuid().ToString();
            //根节点节点
            if (node is GraphLogicRootNode)
            {
                DecGroup decType = (DecGroup)Enum.Parse(typeof(DecGroup), graph.displayName);
                nodeId = ((int)decType).ToString();
            }
            node.id = nodeId;
        }
    }
}
