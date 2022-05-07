using LCNode.Model;
using LCNode.Model.Internal;
using LCToolkit;
using UnityEngine;

namespace LCSkill.BuffGraph
{
    [CreateAssetMenu(fileName = "Buff组", menuName = "配置组/Buff组", order = 1)]
    public class BuffGraphGroupAsset : BaseGraphGroupAsset<BuffGraphAsset>
    {
        public override string DisplayName => "Buff";

        public override void OnClickCreateBtn()
        {
            MiscHelper.Input($"输入BuffId：", (string x) =>
            {
                int buffId = int.Parse(x);
                string assetName = "buff_" + buffId;
                BuffGraphAsset asset = CreateGraph(assetName) as BuffGraphAsset;
                asset.buffId = buffId;
            });
        }

        public override void ExportGraph(InternalBaseGraphAsset graph)
        {

        }
    }
}
