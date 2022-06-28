using LCECS.Core.Tree.Base;
using LCECS.Data;

namespace Demo.Decision
{
    /// <summary>
    /// 检测超过背包上限
    /// </summary>
    public class DEC_PRE_CheckBagItemOutMax : NodePremise
    {
        public int itemId;

        public override bool OnMakeTrue(NodeData wData)
        {
            EntityWorkData workData = wData as EntityWorkData;

            BagCom bagCom = workData.MEntity.GetCom<BagCom>();
            if (bagCom == null)
            {
                GameLocate.Log.LogError("检测超过背包上限失败，没有对应组件", wData.Uid);
                return true;
            }

            return bagCom.CheckItemIsOutMax(itemId);
        }
    }
}