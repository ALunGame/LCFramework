using LCNode.View;
using System.Collections.Generic;

namespace LCECS.Tree
{
    public class BehaviorView : BaseGraphView
    {
        protected override List<string> NodeNamespace => new List<string>() { "LCECS.Tree" };
    } 
}
