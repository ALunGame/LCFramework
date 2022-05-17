using LCECS.Core;
using LCNode.Model;
using System;
using System.Collections.Generic;
using UnityEngine;
using LCToolkit;

namespace LCECS.EntityGraph
{
    public class EntityGraphAsset : BaseGraphAsset<EntityGraph>
    {
        [ReadOnly]
        [Header("实体Id")]
        public int entityId;
    }
}
