using Demo;
using LCECS;
using LCECS.Core.Tree;
using LCECS.Core.Tree.Base;
using LCECS.Core.Tree.Nodes.Action;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecNode
{
    //发送请求
    public class DNSendReq : NodeAction
    {
        public BevType ReqType = BevType.None;

        protected override void OnEnter(NodeData wData)
        {
            //ECSLocate.ECSLog.LogR("发送请求>>>>>>", wData.Id,ReqId.ToString());
            //ECSLayerLocate.Request.PushRequest(wData.Id, (int)ReqType);
        }

        protected override int OnRunning(NodeData wData)
        {
            return NodeState.FINISHED;
        }
    }
}
