﻿using LCECS.Core.Tree;
using LCECS.Core.Tree.Base;
using LCECS.Core.Tree.Nodes.Action;
using LCECS.Data;
using System;

namespace Demo.Decision
{
    /// <summary>
    /// 采集
    /// </summary>
    public class DEC_ACT_Collect : NodeAction
    {
        [NonSerialized]
        private ParamData paramData = new ParamData();
        protected override void OnEnter(NodeData wData)
        {
            EntityWorkData workData = wData as EntityWorkData;
            LCECS.ECSLayerLocate.Request.PushRequest(workData.MEntity.Uid, LCECS.RequestId.Collect, paramData);
            GameLocate.Log.Log("采集>>>>>");
        }

        protected override int OnRunning(NodeData wData)
        {
            EntityWorkData workData = wData as EntityWorkData;
            if (workData.CurrReqId == LCECS.RequestId.Collect)
            {
                return NodeState.EXECUTING;
            }
            return NodeState.FINISHED;
        }

        protected override void OnExit(NodeData wData, int runningStatus)
        {
            EntityWorkData workData = wData as EntityWorkData;
            workData.ChangeRequestId(LCECS.RequestId.None);
        }
    }
}
