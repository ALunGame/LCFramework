using LCJson;
using LCNode;
using LCNode.Model;
using LCNode.Model.Internal;
using LCToolkit;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LCSkill.BulletGraph
{
    [CreateAssetMenu(fileName = "Bullet组", menuName = "配置组/Bullet组", order = 1)]
    public class BulletGraphGroupAsset : BaseGraphGroupAsset<BulletGraphAsset>
    {
        public override string DisplayName => "Bullet";

        public override void OnClickCreateBtn()
        {
            MiscHelper.Input($"输入BulletId：", (string x) =>
            {
                string assetName = "bullet_" + x;
                BulletGraphAsset asset = CreateGraph(assetName) as BulletGraphAsset;
                asset.bulletId = x;
            });
        }

        public override void ExportGraph(List<InternalBaseGraphAsset> assets)
        {
            for (int i = 0; i < assets.Count; i++)
            {
                if (assets[i] is BulletGraphAsset)
                {
                    BulletGraphAsset asset = assets[i] as BulletGraphAsset;

                    BaseGraph graphData = asset.DeserializeGraph();

                    //运行时数据结构
                    BulletModel model = SerializeToBulletModel(graphData, asset);

                    string filePath = SkillDef.GetBulletCnfPath(asset.bulletId);
                    IOHelper.WriteText(JsonMapper.ToJson(model), filePath);

                    Debug.Log($"Bullet配置生成成功>>>>{filePath}");
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private BulletModel SerializeToBulletModel(BaseGraph graph, BulletGraphAsset asset)
        {
            List<Bullet_Node> rootNodes = NodeHelper.GetNodes<Bullet_Node>(graph);
            if (rootNodes.Count <= 0)
            {
                Debug.LogError($"试图序列化出错，没有根节点");
            }
            Bullet_Node node = rootNodes[0];

            BulletModel buletModel  = new BulletModel();
            buletModel.id           = asset.bulletId;
            buletModel.asset = node.asset;
            buletModel.radius = node.radius;
            buletModel.hitTimes = node.hitTimes;
            buletModel.sameTargetDelay = node.sameTargetDelay;
            buletModel.removeOnObstacle = node.removeOnObstacle;
            buletModel.hitEnemy = node.hitEnemy;
            buletModel.hitFriend = node.hitFriend;

            buletModel.moveFunc = node.GetMoveFunc();
            buletModel.catchFunc = node.GetCatchFunc();
            buletModel.onCreateFunc = node.GetOnCreateFuncs();
            buletModel.onRemovedFunc = node.GetOnRemovedFuncs();
            buletModel.onHitFunc = node.GetOnHitFuncs();

            return buletModel;
        }
    }
}
