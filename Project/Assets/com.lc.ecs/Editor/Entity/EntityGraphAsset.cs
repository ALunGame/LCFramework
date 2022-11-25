using LCNode.Model;
using UnityEngine;

namespace LCECS.EntityGraph
{
    public class EntityGraphAsset : BaseGraphAsset<EntityGraph>
    {
        [Header("实体Id")]
        public int entityId;
    }
}
