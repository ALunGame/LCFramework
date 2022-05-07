using LCNode.Model;
using LCNode.Model.Internal;
using LCToolkit;
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
                int buffId = int.Parse(x);
                string assetName = "bullet_" + buffId;
                BulletGraphAsset asset = CreateGraph(assetName) as BulletGraphAsset;
                asset.bulletId = buffId;
            });
        }

        public override void ExportGraph(InternalBaseGraphAsset graph)
        {

        }
    }
}
