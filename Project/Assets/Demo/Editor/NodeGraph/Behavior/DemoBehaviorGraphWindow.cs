using LCECS.NodeGraph.Logic.Node;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XPToolchains.NodeGraph;

namespace Demo.NodeGraph.Behavior
{
    public class DemoBehaviorGraphWindow : BehaviorGraphWindow
    {
        [MenuItem("行为/编辑")]
        public static DemoBehaviorGraphWindow OpenWindow()
        {
            DemoBehaviorGraphWindow window = GetWindow<DemoBehaviorGraphWindow>();
            window.InitGraph(new List<string>() { "NodeGraph.Logic.Node", "Demo.NodeGraph.Behavior", "Demo.NodeGraph.Premise" });
            window.titleContent = new GUIContent("行为编辑");
            window.Show();
            return window;
        }

        protected override void GetNodeId(BaseGraph graph, BaseNode node)
        {
            string nodeId = Guid.NewGuid().ToString();
            //根节点节点
            if (node is GraphLogicRootNode)
            {
                BevType bevType = (BevType)Enum.Parse(typeof(BevType), graph.displayName);
                nodeId = ((int)bevType).ToString();
            }
            node.id = nodeId;
        }
    } 
}
