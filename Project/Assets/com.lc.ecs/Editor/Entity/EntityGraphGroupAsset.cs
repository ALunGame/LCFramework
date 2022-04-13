using LCECS.Model;
using LCHelp;
using LCJson;
using LCNode;
using LCNode.Model;
using LCNode.Model.Internal;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LCECS.EntityGraph
{
    [CreateAssetMenu(fileName = "实体组", menuName = "配置组/实体组", order = 1)]
    public class EntityGraphGroupAsset : BaseGraphGroupAsset<EntityGraphAsset>
    {
        public override string DisplayName => "实体配置";

        public static EntityModel SerializeToEntityModel(BaseGraph graph)
        {
            List<Entity_Node> rootNodes = NodeHelper.GetNodes<Entity_Node>(graph);
            if (rootNodes.Count <= 0)
            {
                Debug.LogError($"试图序列化出错，没有根节点");
            }
            EntityModel model = rootNodes[0].GetModel();
            return model;
        }

        public override void ExportGraph(InternalBaseGraphAsset graph)
        {
            EntityGraphAsset entityAsset = graph as EntityGraphAsset;
            BaseGraph graphData = entityAsset.DeserializeGraph();

            //运行时数据结构
            EntityModel model = SerializeToEntityModel(graphData);

            string filePath = ECSDefPath.GetEntityPath(entityAsset.name);
            LCIO.WriteText(JsonMapper.ToJson(model), filePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"实体配置生成成功>>>>{filePath}");
        }
    }
}
