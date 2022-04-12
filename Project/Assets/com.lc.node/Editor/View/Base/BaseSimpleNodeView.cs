using LCNode.Model;
using LCNode.View.Utils;
using UnityEngine.UIElements;

namespace LCNode.View
{
    public abstract class BaseSimpleNodeView<M> : BaseNodeView<M> where M : BaseNode
    {
        protected BaseSimpleNodeView() : base()
        {
            styleSheets.Add(GraphProcessorStyles.SimpleNodeViewStyle);
            m_CollapseButton.style.display = DisplayStyle.None;
        }
    }

    public class BaseSimpleNodeView : BaseSimpleNodeView<BaseNode> { }
}
