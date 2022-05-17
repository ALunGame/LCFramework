using LCNode;
using LCNode.View;
using UnityEngine.UIElements;
using LCSkill.AoeGraph;

namespace LCSkill.BuffGraph
{
    [CustomNodeView(typeof(Buff_LifeCycleAddAoeNode))]
    public class Buff_LifeCycleAddAoeNodeView : BaseNodeView
    {
        Button btnOpenBuff;
        public Buff_LifeCycleAddAoeNodeView()
        {
            btnOpenBuff = new Button();
            btnOpenBuff.text = "打开Aoe";
            controlsContainer.Add(btnOpenBuff);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            btnOpenBuff.clicked += OnClickOpenBuff;
        }

        private void OnClickOpenBuff()
        {
            Buff_LifeCycleAddAoeNode node = Model as Buff_LifeCycleAddAoeNode;
            GraphGroupPath path = GraphSetting.Setting.GetSearchPath<AoeGraphGroupAsset>();
            AoeGraphAsset asset = GraphSetting.Setting.GetAsset<AoeGraphAsset>(path.searchPath, "aoe_" + node.addAoe.id);
            if (asset != null)
            {
                BaseGraphWindow.JumpTo(asset);
            }
        }
    }
}