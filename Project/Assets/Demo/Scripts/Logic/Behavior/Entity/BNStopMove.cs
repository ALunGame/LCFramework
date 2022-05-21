using Demo.Com;
using LCECS;
using LCECS.Core.Tree.Base;
using LCECS.Core.Tree.Nodes.Action;
using LCECS.Data;

namespace Demo.BevNode
{
    public class BNStopMove : NodeAction
    {
        protected override void OnEnter(NodeData wData)
        {
            EntityWorkData workData         = ECSLocate.Player.GetPlayerWorkData();
            SpeedCom speedCom               = workData.MEntity.GetCom<SpeedCom>();
            //赋值组件
            speedCom.ReqMoveSpeed           = 0;
            speedCom.ReqJumpSpeed           = 0;
            speedCom.ReqDash                = false;

            SeekPathCom seekPathCom = workData.MEntity.GetCom<SeekPathCom>();
            if (seekPathCom != null)
            {
                seekPathCom.ReqSeek = false;
                seekPathCom.TargetPos = seekPathCom.CurrPos;
                seekPathCom.MovePath = null;
                seekPathCom.IsSeeking = false;
            }
        }
    }
}
