using Demo.Com;
using LCECS;
using LCECS.Core.Tree;
using LCECS.Core.Tree.Base;
using LCECS.Core.Tree.Nodes.Action;
using LCECS.Data;
using UnityEngine;

namespace Demo.BevNode
{
    /// <summary>
    /// 寻路节点
    /// </summary>
    [Node(ViewName = "寻路节点", IsBevNode = true)]
    public class BNSeekPath : NodeAction
    {
        protected override void OnEnter(NodeData wData)
        {
            EntityWorkData workData = wData as EntityWorkData;
            EnemyCom enemyCom = workData.MEntity.GetCom<EnemyCom>();
            SeekPathCom seekPathCom = workData.MEntity.GetCom<SeekPathCom>();
            
            //参数
            ParamData param         = workData.GetReqParam(workData.CurrReqId);
            Vector2Int targetPos    = param.GetVect2Int();
            bool isWander           = param.GetBool();
            
            //徘徊处理
            if (isWander)
            {
                Vector2Int wanderPos = Vector2Int.zero;
                if (enemyCom.WanderIndex >= enemyCom.WanderPath.Count)
                {
                    wanderPos = enemyCom.SpawnPos;
                }
                else
                {
                    wanderPos = enemyCom.WanderPath[enemyCom.WanderIndex];
                    if (seekPathCom.TargetPos.Equals(wanderPos))
                    {
                        enemyCom.WanderIndex++;
                        if (enemyCom.WanderIndex > enemyCom.WanderPath.Count - 1)
                        {
                            enemyCom.WanderIndex = 0;
                        }
                        wanderPos = enemyCom.WanderPath[enemyCom.WanderIndex];
                    }
                }

                targetPos = wanderPos;
            }

            //对组件赋值
            if (!seekPathCom.TargetPos.Equals(targetPos))
            {
                seekPathCom.TargetPos = targetPos;
                seekPathCom.ReqSeek = true;
                seekPathCom.IsSeekHasNoWay = false;
            }
        }

        protected override int OnRunning(NodeData wData)
        {
            EntityWorkData workData = wData as EntityWorkData;
            SeekPathCom seekPathCom = workData.MEntity.GetCom<SeekPathCom>();

            //没有路
            if (seekPathCom.IsSeekHasNoWay)
            {
                return NodeState.FINISHED;
            }
            
            //没有达到目标点
            if (!seekPathCom.CurrPos.Equals(seekPathCom.TargetPos))
            {
                return NodeState.EXECUTING;
            }
            
            return NodeState.FINISHED;
        }
    }
}