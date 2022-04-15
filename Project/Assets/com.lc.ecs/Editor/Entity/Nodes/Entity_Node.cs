using LCECS.Core;
using LCNode;
using LCNode.Model;
using System.Collections.Generic;
using UnityEngine;

namespace LCECS.EntityGraph
{
    public class EntityComData { }

    public abstract class Entity_ComNode : BaseNode
    {
        public override Color TitleColor { get => Color.magenta; set => base.TitleColor = value; }

        [InputPort("父节点", BasePort.Capacity.Single, BasePort.Orientation.Vertical)]
        public EntityComData parentNode;

        public abstract BaseCom CreateRuntimeNode();
    }

    [NodeMenuItem("实体配置")]
    public class Entity_Node : BaseNode
    {
        public override string Title { get => "实体配置"; set => base.Title = value; }
        public override string Tooltip { get => "实体配置"; set => base.Tooltip = value; }
        public override Color TitleColor { get => Color.white; set => base.TitleColor = value; }

        [OutputPort("组件列表", BasePort.Capacity.Multi, BasePort.Orientation.Vertical)]
        public EntityComData coms;

        [NodeValue("实体Id")]
        public int id = 0;

        [NodeValue("实体名")]
        public string name = "";

        [NodeValue("决策树Id")]
        public int decTreeId = 0;

        [NodeValue("预制体路径")]
        public string prefabPath = "";

        public Entity GetModel()
        {
            List<BaseCom> coms = new List<BaseCom>();
            //组件节点
            List<Entity_ComNode> nodes = NodeHelper.GetNodeOutNodes<Entity_ComNode>(Owner, this);
            if (nodes.Count > 0)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    coms.Add(nodes[i].CreateRuntimeNode());
                }
            }
            Entity model = new Entity(id, name, decTreeId, prefabPath, coms);
            return model;
        }
    }
}
