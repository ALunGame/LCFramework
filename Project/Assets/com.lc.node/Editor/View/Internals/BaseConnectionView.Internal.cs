using LCNode.Model;
using LCNode.View.Utils;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;


namespace LCNode.View
{
    /// <summary>
    /// 连接显示
    /// </summary>
    public partial class BaseConnectionView : Edge, IBindableView<BaseConnection>
    {
        public BaseConnection Model { get; private set; }
        protected BaseGraphView Owner { get; private set; }

        public BaseConnectionView() : base()
        {
            styleSheets.Add(GraphProcessorStyles.EdgeViewStyle);
            this.RegisterCallback<MouseEnterEvent>(OnMouseEnter);
        }

        public void SetUp(BaseConnection connection, BaseGraphView graphView)
        {
            Model = connection;
            Owner = graphView;
        }

        public virtual void UnBindingProperties()
        {

        }

        private void OnMouseEnter(MouseEnterEvent evt)
        {
            this.BringToFront();
        }
    }
}
