using LCNode.Model;
using UnityEngine;

namespace LCECS.EntityGraph
{
    public class EntityGraphAsset : BaseGraphAsset<EntityGraph>
    {
        [Header("实体Id")]
        public int entityId;

        [Header("实体名")]
        public string entityName;
    }
}
