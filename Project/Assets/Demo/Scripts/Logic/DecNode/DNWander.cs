using LCECS;
using LCECS.Core.Tree.Base;
using LCECS.Core.Tree.Nodes.Action;
using LCECS.Data;

namespace Demo.DecNode
{
    public class DNWander : NodeAction
    {
        protected override void OnEnter(NodeData wData)
        {
            EntityWorkData workData = wData as EntityWorkData;
            //发送请求
            ParamData paramData = workData.GetReqParam((int)BevType.SeekPath);
            //徘徊标识
            paramData.SetBool(true);
            ECSLayerLocate.Request.PushRequest(workData.MEntity.GetHashCode(), (int)BevType.SeekPath);
        }
    }
}
