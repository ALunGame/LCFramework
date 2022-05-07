using LCNode.Model;
using LCNode.Model.Internal;
using LCToolkit;
using UnityEngine;

namespace LCSkill.AoeGraph
{
    [CreateAssetMenu(fileName = "Aoe组", menuName = "配置组/Aoe组", order = 1)]
    public class AoeGraphGroupAsset : BaseGraphGroupAsset<AoeGraphAsset>
    {
        public override string DisplayName => "Aoe";

        public override void OnClickCreateBtn()
        {
            MiscHelper.Input($"输入AoeId：", (string x) =>
            {
                int buffId = int.Parse(x);
                string assetName = "aoe_" + buffId;
                AoeGraphAsset asset = CreateGraph(assetName) as AoeGraphAsset;
                asset.aoeId = buffId;
            });
        }

        public override void ExportGraph(InternalBaseGraphAsset graph)
        {

        }
    }
}
