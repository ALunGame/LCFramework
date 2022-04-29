using LCECS.Core.Tree.Base;

namespace LCECS.Core.Tree.Nodes.Action
{
    /// <summary>
    /// 实体请求行为           
    /// </summary>
    public class BNReqRequset : NodeAction
    {
        public int ReqId = 0;

        protected override void OnEnter(NodeData wData)
        {
            //ECSLocate.ECSLog.LogR("发送请求>>>>>>", wData.Id,ReqId.ToString());
            //ECSLayerLocate.Request.PushRequest(wData.Id, ReqId);
        }

        protected override int OnRunning(NodeData wData)
        {
            return NodeState.FINISHED;
        }
    }
}
