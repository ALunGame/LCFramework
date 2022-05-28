using Demo.Com;
using LCECS;
using LCECS.Core.Tree.Base;
using LCECS.Core.Tree.Nodes.Action;
using LCECS.Data;
using System;

namespace Demo.Decision
{
    public class DEC_ACT_Input_Player_Move : NodeAction
    {
        [NonSerialized]
        private ParamData paramData = new ParamData();
        [NonSerialized]
        private InputCom inputCom = null;

        protected override void OnEnter(NodeData wData)
        {
            if (inputCom == null)
                inputCom = ECSLocate.ECS.GetWorld().GetCom<InputCom>();
            paramData.SetVect2(inputCom.Param.GetVect2());
            EntityWorkData workData = wData as EntityWorkData;
            ECSLayerLocate.Request.PushRequest(workData.MEntity.GetHashCode(), RequestId.Move, paramData);
        }
    }
}
