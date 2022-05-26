using Demo.Com;
using LCECS.Core.Tree;
using LCECS.Core.Tree.Base;
using LCECS.Core.Tree.Nodes.Action;
using LCECS.Data;
using UnityEngine;

namespace Demo.Behavior
{
    public class BEV_ACT_GazeSurround : NodeAction
    {
        protected override void OnEnter(NodeData wData)
        {
            EntityWorkData workData = wData as EntityWorkData;
            //参数
            ParamData paramData = workData.GetParam();
            int gazeUid = paramData.GetInt();
            Vector2 gazeRange = paramData.GetVect2();
            //组件
            GazeSurroundCom gazeSurroundCom = workData.MEntity.GetCom<GazeSurroundCom>();
            gazeSurroundCom.gazeUid = gazeUid;
            gazeSurroundCom.gazeRange = gazeRange;
            gazeSurroundCom.Enable();
            Debug.LogError("BEV_ACT_GazeSurround>>>>OnEnter");
        }

        protected override int OnRunning(NodeData wData)
        {
            return NodeState.EXECUTING;
        }

        protected override void OnExit(NodeData wData, int runningStatus)
        {
            EntityWorkData workData = wData as EntityWorkData;
            //组件
            GazeSurroundCom gazeSurroundCom = workData.MEntity.GetCom<GazeSurroundCom>();
            gazeSurroundCom.Disable();
            Debug.LogError("BEV_ACT_GazeSurround>>>>OnExit");
        }
    }
}
