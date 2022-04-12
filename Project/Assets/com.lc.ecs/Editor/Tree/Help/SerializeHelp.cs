using LCECS.Core.Tree.Base;
using LCNode;
using LCNode.Model;
using System.Collections.Generic;
using UnityEngine;

namespace LCECS.Tree
{
    /// <summary>
    /// 序列化辅助
    /// </summary>
    public static class SerializeHelp
    {
        public static Node SerializeToTree(BaseGraph graph)
        {
            List<Tree_RootNode> rootNodes = NodeHelper.GetNodes<Tree_RootNode>(graph);
            if (rootNodes.Count <= 0)
            {
                Debug.LogError($"试图序列化出错，没有根节点");
            }
            Node rootNode = rootNodes[0].GetRuntimeNode();
            return rootNode;
        }
    }
}
