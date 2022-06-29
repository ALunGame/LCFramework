using Demo.Com;
using LCECS.Core.Tree.Base;
using LCECS.Data;

namespace Demo.Decision
{
    /// <summary>
    /// 采集物品超过上限
    /// </summary>
    public class DEC_PRE_CheckCollectItemIsOutMax : NodePremise
    {
        public override bool OnMakeTrue(NodeData wData)
        {
            EntityWorkData workData = wData as EntityWorkData;

            CollectCom collectCom = workData.MEntity.GetCom<CollectCom>();
            if (collectCom == null)
            {
                GameLocate.Log.LogError("检测采集物品超过上限失败，没有对应组件", wData.Uid);
                return true;
            }

            return collectCom.collectItem.CheckIsOutMax();
        }
    }

    /// <summary>
    /// 采集物品还有剩余
    /// </summary>
    public class DEC_PRE_CheckCollectItemHasLeft : NodePremise
    {
        public override bool OnMakeTrue(NodeData wData)
        {
            EntityWorkData workData = wData as EntityWorkData;

            CollectCom collectCom = workData.MEntity.GetCom<CollectCom>();
            if (collectCom == null)
            {
                GameLocate.Log.LogError("检测采集物品还有剩余失败，没有对应组件", wData.Uid);
                return true;
            }

            return collectCom.collectItem.cnt > 0;
        }
    }
}
