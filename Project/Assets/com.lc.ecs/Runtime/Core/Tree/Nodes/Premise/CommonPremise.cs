using LCECS.Core.Tree.Base;
using LCECS.Data;

namespace LCECS.Core.Tree.Nodes.Premise
{
    /// <summary>
    /// 检测实体Id
    /// </summary>
    public class BEV_PRE_CheckEntityId : NodePremise
    {
        public int entityId;
        public override bool OnMakeTrue(NodeData wData)
        {
            EntityWorkData workData = wData as EntityWorkData;
            return workData.MEntity.EntityId == entityId;
        }
    }
}