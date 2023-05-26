using System;
using System.Collections.Generic;
using UnityEngine;
using MemoryPack;

namespace LCNode.Model
{
    public partial class BaseGraph
    {
        //位置
        internal Vector3 pos = Vector3.zero;
        //缩放
        internal Vector3 zoom = Vector3.one;

        //节点列表
        internal Dictionary<string, BaseNode> nodes = new Dictionary<string, BaseNode>();
        
        //组
        internal List<BaseGroup> groups = new List<BaseGroup>();

        //连接
        internal List<BaseConnection> connections = new List<BaseConnection>();
    }
}
