using System;
using System.Collections.Generic;
using LCECS.Core.Tree.Base;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LCECS.Core.Tree.Nodes.Control
{
    /// <summary>
    /// 随机控制选择节点    （随机一个子节点可运行，就直接运行）
    /// </summary>
    public class NodeControlRandomSelector : NodeControl
    {
        class NodeControlRandomSelectorContext : NodeContext
        {
            internal int currentSelectedIndex;
            internal int lastSelectedIndex;

            public NodeControlRandomSelectorContext()
            {
                currentSelectedIndex = -1;
                lastSelectedIndex = -1;
            }
        }
        
        protected override bool OnEvaluate(NodeData wData)
        {
            NodeControlRandomSelectorContext thisContext = GetContext<NodeControlRandomSelectorContext>(wData);
            thisContext.currentSelectedIndex = -1;

            //寻找可以运行的子节点
            List<Node> canRunChildNodes = new List<Node>();
            int childCount = GetChildCount();
            for (int i = 0; i < childCount; ++i)
            {
                Node node = GetChild<Node>(i);
                if (node.Evaluate(wData))
                {
                    canRunChildNodes.Add(node);
                }
            }

            //没有可运行的
            if (canRunChildNodes.Count<=0)
            {
                return false;
            }
            
            thisContext.currentSelectedIndex = Random.Range(0,canRunChildNodes.Count);
            return true;
        }

        protected override int OnExcute(NodeData wData)
        {
            NodeControlRandomSelectorContext thisContext = GetContext<NodeControlRandomSelectorContext>(wData);
            int runningState = NodeState.FINISHED;

            //当前选择的不是上次选择的 （执行下上次的切换方法）
            if (thisContext.currentSelectedIndex != thisContext.lastSelectedIndex)
            {
                if (IsIndexValid(thisContext.lastSelectedIndex))
                {
                    Node node = GetChild<Node>(thisContext.lastSelectedIndex);
                    node.Transition(wData);
                }
                thisContext.lastSelectedIndex = thisContext.currentSelectedIndex;
            }

            //执行下选择的子节点
            if (IsIndexValid(thisContext.lastSelectedIndex))
            {
                Node node = GetChild<Node>(thisContext.lastSelectedIndex);
                runningState = node.Execute(wData);
                if (NodeState.IsFinished(runningState))
                {
                    thisContext.lastSelectedIndex = -1;
                }
            }
            return runningState;
        }

        protected override void OnTransition(NodeData wData)
        {
            NodeControlRandomSelectorContext thisContext = GetContext<NodeControlRandomSelectorContext>(wData);
            Node node = GetChild<Node>(thisContext.lastSelectedIndex);
            if (node != null)
            {
                node.Transition(wData);
            }
            thisContext.lastSelectedIndex = -1;
        }

    }
}