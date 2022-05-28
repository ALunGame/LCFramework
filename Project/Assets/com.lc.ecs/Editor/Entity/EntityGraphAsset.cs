using LCNode.Model;
using UnityEngine;

namespace LCECS.EntityGraph
{
    public class EntityGraphAsset : BaseGraphAsset<EntityGraph>
    {
        [Header("实体Id")]
        public int entityId;

        [Header("决策Id")]
        public int decTree = 0;

        [Header("决策组")]
        public DecisionGroup decGroup = DecisionGroup.HighThread;
    }
}
