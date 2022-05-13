using LCECS.Core;
using LCToolkit;
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

        public override void OnClickCreateBtn()
        {
            MiscHelper.Input($"输入实体Id：", (string x) =>
            {
                int entityId = int.Parse(x);
                string assetName = "entity_" + entityId;
                EntityGraphAsset asset = CreateGraph(assetName) as EntityGraphAsset;
                asset.entityId = entityId;
            });
        }

        public override void ExportGraph(List<InternalBaseGraphAsset> assets)
        {
            for (int i = 0; i < assets.Count; i++)
            {
                EntityGraphAsset entityAsset = assets[i] as EntityGraphAsset;
                BaseGraph graphData = entityAsset.DeserializeGraph();

                //运行时数据结构
                Entity model = SerializeToEntityModel(graphData, entityAsset);

                string filePath = ECSDefPath.GetEntityPath(entityAsset.entityId);
                IOHelper.WriteText(JsonMapper.ToJson(model), filePath);
                Debug.Log($"实体配置生成成功>>>>{filePath}");
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private Entity SerializeToEntityModel(BaseGraph graph, EntityGraphAsset entityAsset)
        {
            List<Entity_Node> rootNodes = NodeHelper.GetNodes<Entity_Node>(graph);
            if (rootNodes.Count <= 0)
            {
                Debug.LogError($"试图序列化出错，没有根节点");
            }
            List<BaseCom> coms = rootNodes[0].GetModel();
            Entity model = new Entity(entityAsset.entityId, rootNodes[0].name, rootNodes[0].decTreeId, coms);
            return model;
        }
    }
}
