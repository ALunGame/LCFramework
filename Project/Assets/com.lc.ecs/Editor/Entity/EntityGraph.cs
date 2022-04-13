using LCNode.Model;
using System.Collections.Generic;

namespace LCECS.EntityGraph
{
    public class EntityGraph : BaseGraph
    {
        public override List<string> NodeNamespace => new List<string>() { "LCECS.EntityGraph" };
    }
}