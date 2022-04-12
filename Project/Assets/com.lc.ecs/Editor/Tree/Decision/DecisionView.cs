using LCNode.View;
using System.Collections.Generic;

namespace LCECS.Tree
{
    public class DecisionView : BaseGraphView
    {
        protected override List<string> NodeNamespace => new List<string>() { "LCECS.Tree" };
    } 
}
