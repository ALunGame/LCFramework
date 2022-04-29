using Demo.Com;
using LCECS.Core.Tree.Base;
using LCECS.Core.Tree.Nodes.Action;
using LCECS.Data;
using UnityEngine;

namespace Demo.BevNode
{
    /// <summary>
    /// 移动行为
    /// </summary>
    public class BEV_ACT_Move : NodeAction
    {
        protected override void OnEnter(NodeData wData)
        {
            EntityWorkData workData = wData as EntityWorkData;

            Vector2 inputMove = workData.GetReqParam(workData.CurrReqId).GetVect2();

            MoveCom moveCom = workData.MEntity.GetCom<MoveCom>();
            PropertyCom propertyCom = workData.MEntity.GetCom<PropertyCom>();

            float yValue = inputMove.y == 0 ? moveCom.Velocity.y: inputMove.y;
            Vector2 velocity = new Vector2(propertyCom.MoveSpeed.Curr * inputMove.x, yValue);
            moveCom.Velocity = velocity;
        }
    }
}