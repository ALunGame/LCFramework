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

            //CollectCom collectCom = workData.MEntity.GetCom<CollectCom>();
            //BagCom bagCom = workData.MEntity.GetCom<BagCom>();
            //if (collectCom == null || bagCom == null)
            //{
            //    GameLocate.Log.LogError("检测采集物品超过上限失败，没有对应组件", wData.Uid);
            //    return true;
            //}

            //return bagCom.CheckItemIsOutMax(collectCom.collectActorId);

            return false;
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

            //CollectCom collectCom = workData.MEntity.GetCom<CollectCom>();
            //BagCom bagCom = workData.MEntity.GetCom<BagCom>();
            //if (collectCom == null || bagCom == null)
            //{
            //    GameLocate.Log.LogError("检测采集物品还有剩余失败，没有对应组件", wData.Uid);
            //    return true;
            //}

            //return bagCom.GetBagItem(collectCom.collectActorId).cnt > 0;

            return false;
        }
    }
}
