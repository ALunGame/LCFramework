using Demo.Com;
using LCECS.Core.Tree.Base;
using LCECS.Data;
using System.Collections.Generic;

namespace Demo.Decision
{
    /// <summary>
    /// 检测是否可以生产
    /// </summary>
    public class DEC_PRE_CheckCanProduce : NodePremise
    {
        public override bool OnMakeTrue(NodeData wData)
        {
            //EntityWorkData workData = wData as EntityWorkData;

            //ProduceCom produceCom   = workData.MEntity.GetCom<ProduceCom>();
            //ManagerCom managerCom   = workData.MEntity.GetCom<ManagerCom>();

            //if (produceCom == null || managerCom == null)
            //{
            //    GameLocate.Log.LogError("检测是否可以生产失败，没有对应组件", wData.Uid);
            //    return true;
            //}

            //BagCom bagCom = managerCom.buildingBagCom;
            //List<int> resProduces = produceCom.GetCanMakeProduceIds(bagCom);
            //if (resProduces.Count == 0)
            //{
            //    return false;
            //}

            return true;
        }
    }
}
