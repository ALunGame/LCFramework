using LCNode;
using LCNode.View;
using UnityEngine.UIElements;

namespace LCSkill.BuffGraph
{
    [CustomNodeView(typeof(Buff_LifeCycleAddBuffNode))]
    public class Buff_LifeCycleAddBuffNodeView : BaseNodeView
    {
        Button btnOpenBuff;
        public Buff_LifeCycleAddBuffNodeView()
        {
            btnOpenBuff = new Button();
            btnOpenBuff.text = "打开Buff";
            controlsContainer.Add(btnOpenBuff);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            btnOpenBuff.clicked += OnClickOpenBuff;
        }

        private void OnClickOpenBuff()
        {
            Buff_LifeCycleAddBuffNode node = Model as Buff_LifeCycleAddBuffNode;
            GraphGroupPath path = GraphSetting.Setting.GetSearchPath<BuffGraphGroupAsset>();
            BuffGraphAsset asset = GraphSetting.Setting.GetAsset<BuffGraphAsset>(path.searchPath, "buff_" + node.addBuff.id);
            if (asset != null)
            {
                BaseGraphWindow.JumpTo(asset);
            }
        }
    }
}