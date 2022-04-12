using LCNode;
using LCNode.Model;
using LCNode.View;

namespace LCECS.Tree
{
    [CustomGraphWindow(typeof(Behavior))]
    public class BehaviorWindow : BaseGraphWindow
    {
        protected override BaseGraphView NewGraphView(BaseGraph graph)
        {
            return new BehaviorView();
        }
    } 
}
