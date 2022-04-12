using LCNode;
using LCNode.Model;
using LCNode.View;

namespace LCECS.Tree
{
    [CustomGraphWindow(typeof(Decision))]
    public class DecisionWindow : BaseGraphWindow
    {
        protected override BaseGraphView NewGraphView(BaseGraph graph)
        {
            return new DecisionView();
        }
    } 
}
