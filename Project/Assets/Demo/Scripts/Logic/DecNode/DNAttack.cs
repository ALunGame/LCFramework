using Demo.Com;
using LCECS;
using LCECS.Core.Tree.Base;
using LCECS.Core.Tree.Nodes.Action;
using LCECS.Data;
using UnityEngine;

namespace DecNode
{
    [Node(ViewName = "攻击", IsBevNode = false)]
    public class DNAttack : NodeAction
    {
        protected override void OnEnter(NodeData wData)
        {
            EntityWorkData workData = wData as EntityWorkData;
            EnemyCom seekPathCom = workData.MEntity.GetCom<EnemyCom>();

            Vector2Int targrtPos = Vector2Int.zero;

            if (seekPathCom.WanderPath[seekPathCom.WanderIndex] == null)
            {
                targrtPos = seekPathCom.SpawnPos;
            }
            else
            {
                targrtPos = seekPathCom.WanderPath[seekPathCom.WanderIndex];
            }
            
            //发送请求
            ECSLocate.ECSLog.LogError("敌人攻击");
        }
    }
}
