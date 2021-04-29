using LCECS.Core.Tree.Base;
using LCECS.NodeGraph.Logic.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using XPToolchains.Help;
using XPToolchains.NodeGraph;

namespace LCECS.NodeGraph.Serialize
{
    public static class GraphSerializeToTree
    {
        public static BaseNode GetNodeByType(BaseGraph baseGraph,Type nodeType)
        {
            for (int i = 0; i < baseGraph.nodes.Count; i++)
            {
                if (baseGraph.nodes[i].GetType() == nodeType)
                {
                    return baseGraph.nodes[i];
                }
            }
            return null;
        }

        //获得这个节点的输出节点
        public static List<BaseNode> GetNodeOutNodes(BaseGraph baseGraph, BaseNode checkNode)
        {
            List<BaseNode> childNodes = new List<BaseNode>();
            for (int i = 0; i < baseGraph.edges.Count; i++)
            {
                BaseEdge baseEdge = baseGraph.edges[i];
                if (baseEdge.outputNodeGUID == checkNode.id)
                {
                    BaseNode inputNode = baseGraph.nodes.First(v => v.id == baseEdge.inputNodeGUID);
                    if (inputNode != null)
                    {
                        childNodes.Add(inputNode);
                    }
                }
            }
            return childNodes;
        }

        //获得这个节点的输入节点
        public static List<BaseNode> GetNodeInNodes(BaseGraph baseGraph, BaseNode checkNode)
        {
            List<BaseNode> childNodes = new List<BaseNode>();
            for (int i = 0; i < baseGraph.edges.Count; i++)
            {
                BaseEdge baseEdge = baseGraph.edges[i];
                if (baseEdge.inputNodeGUID == checkNode.id)
                {
                    BaseNode outputNode = baseGraph.nodes.FirstOrDefault(v => v.id == baseEdge.outputNodeGUID);
                    if (outputNode != null)
                    {
                        childNodes.Add(outputNode);
                    }
                }
            }
            return childNodes;
        }

        private static void setNodeParam(object node,BaseNode baseNode)
        {
            foreach (FieldInfo fInfo in baseNode.GetType().GetFields())
            {
                InputAttribute inputAttr = (InputAttribute)fInfo.GetCustomAttribute(typeof(InputAttribute));
                if (inputAttr == null)
                    continue;

                if (fInfo.FieldType == typeof(GraphLogicNodeData) || fInfo.FieldType == typeof(GraphLogicPremiseData))
                    continue;

                string valueName    = fInfo.Name;
                object value        = fInfo.GetValue(baseNode);

                foreach (var field in node.GetType().GetFields())
                {
                    if (field.Name == valueName)
                    {
                        try
                        {
                            field.SetValue(node, value);
                        }
                        catch (Exception e)
                        {
                            Debug.LogError($"SetValue>>>>>--{baseNode.name}:{valueName}>>{field.Name}");
                            Debug.LogError($"Exception：{e}");
                        }
                    }
                }
            }
        }

        private static NodePremise genNodePremise(BaseNode premiseNode, BaseNode baseNode, NodePremise parentPremise)
        {
            if (premiseNode == null)
                return null;
            NodeMenuItemAttribute nodeAttr = LCHelp.LCReflect.GetTypeAttr<NodeMenuItemAttribute>(premiseNode.GetType());
            if (nodeAttr == null)
                return null;

            //创建实例
            Type nodeType = nodeAttr.serializeType;
            NodePremise nodePremise = Activator.CreateInstance(nodeType) as NodePremise;
            nodePremise.Uid = baseNode.id;
            //参数
            setNodeParam(nodePremise, premiseNode);
            if (parentPremise != null)
                parentPremise.AddOtherPrecondition(nodePremise);
            return nodePremise;
        }

        private static void CreatePremise(BaseNode checkNode, NodePremise paremtPremise,BaseGraph graph)
        {
            BaseNode nextNodePremise    = null;
            List<BaseNode> inputNodes   = GetNodeInNodes(graph, checkNode);
            for (int i = 0; i < inputNodes.Count; i++)
            {
                if (inputNodes[i].GetType().BaseType == typeof(GraphLogicPremiseNode))
                {
                    nextNodePremise = inputNodes[i];
                    break;
                }
            }
            if (nextNodePremise == null)
                return;

            NodePremise nextPremise = genNodePremise(nextNodePremise, checkNode, paremtPremise);
            CreatePremise(nextNodePremise, nextPremise, graph);
        }


        private static Node genNode(BaseNode baseNode, BaseGraph graph)
        {
            NodeMenuItemAttribute nodeAttr = LCHelp.LCReflect.GetTypeAttr<NodeMenuItemAttribute>(baseNode.GetType());
            if (nodeAttr == null)
                return null;

            //创建实例
            Type nodeType = nodeAttr.serializeType;
            Node treeNode = Activator.CreateInstance(nodeType) as Node;
            treeNode.Uid  = baseNode.id;
            //前提
            BaseNode nodePremise = null;
            List<BaseNode> inputNodes = GetNodeInNodes(graph, baseNode);
            for (int i = 0; i < inputNodes.Count; i++)
            {
                if (inputNodes[i].GetType().BaseType == typeof(GraphLogicPremiseNode))
                {
                    nodePremise = inputNodes[i];
                    break;
                }
            }
            if (nodePremise != null)
            {
                NodePremise premise = genNodePremise(nodePremise, baseNode, null);
                CreatePremise(nodePremise, premise, graph);
                treeNode.SetPremise(premise);
            }
            //参数
            setNodeParam(treeNode, baseNode);
            return treeNode;
        }

        public static void CreateNode(BaseGraph graph,BaseNode parentNodeView, Node parentNode)
        {
            List<BaseNode> childNodes = GetNodeOutNodes(graph, parentNodeView);
            if (childNodes == null || childNodes.Count<=0)
                return;
            for (int i = 0; i < childNodes.Count; i++)
            {
                BaseNode childNodeView = childNodes[i];
                Node childNode = genNode(childNodeView, graph);
                parentNode.AddChild(childNode);
                CreateNode(graph, childNodeView, childNode);
            }
        }

        //从根节点开始
        public static Node SerializeToTree(BaseGraph graph)
        {
            BaseNode rootNodeView = GetNodeByType(graph, typeof(GraphLogicRootNode));
            if (rootNodeView == null)
            {
                Debug.LogError($"试图序列化出错，没有根节点>>>>{graph.displayName}");
                return null;
            }
            Node rootNode = genNode(rootNodeView, graph);
            CreateNode(graph, rootNodeView, rootNode);
            return rootNode;
        }
    }
}
