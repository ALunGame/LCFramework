using LCECS.Core.Tree.Base;
using LCECS.Core.Tree.Nodes.Action;
using LCECS.Data;
using System;

namespace Demo.Decision
{
    /// <summary>
    /// 徘徊
    /// </summary>
    public class DEC_ACT_Wander : NodeAction
    {
        //徘徊范围
        public float WanderRange = 0;

        [NonSerialized]
        private ParamData paramData = new ParamData();
        protected override void OnEnter(NodeData wData)
        {
            EntityWorkData workData = wData as EntityWorkData;
            paramData.SetFloat(WanderRange);

            LCECS.ECSLayerLocate.Request.PushRequest(workData.MEntity.GetHashCode(), LCECS.RequestId.Wander, paramData);
            //发送请求
            //ParamData paramData = workData.GetReqParam((int)BevType.SeekPath);
            //徘徊标识
            //paramData.SetBool(true);
            //ECSLayerLocate.Request.PushRequest(workData.MEntity.GetHashCode(), (int)BevType.SeekPath);
        }
    }
}
